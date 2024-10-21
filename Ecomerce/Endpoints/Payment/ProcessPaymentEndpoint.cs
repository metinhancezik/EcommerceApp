using FastEndpoints;
using ECommerceView.Models;
using Iyzico3DPaymentShared.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Iyzico3DPayment.Shared.Models;
using Iyzico3DPaymentProject.Models;

namespace ECommerceView.Endpoints.Payment;

public class ProcessPaymentEndpoint : Endpoint<PaymentViewModel, PaymentResponse>
{
    private readonly ILogger<ProcessPaymentEndpoint> _logger;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ProcessPaymentEndpoint(ILogger<ProcessPaymentEndpoint> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient();
        _configuration = configuration;
        _httpClient.BaseAddress = new Uri(_configuration["ApiSettings:BaseUrl"]);
    }

    public override void Configure()
    {
        Post("/payment/process");
    }

    public override async Task HandleAsync(PaymentViewModel req, CancellationToken ct)
    {
        try
        {
            _logger.LogInformation("Ödeme işlemi başlatıldı: {PaymentDetails}", req);

            var accountInfo = GetAccountInfo();
            if (accountInfo == null)
            {
                await SendAsync(new PaymentResponse { Status = "error", ErrorMessage = "Kullanıcı bilgileri bulunamadı." }, 400, ct);
                return;
            }

            var requestModel = CreatePaymentRequestModel(req, accountInfo);
            var paymentResponse = await SendPaymentRequest(requestModel);

            if (paymentResponse != null)
            {
                await SendAsync(paymentResponse, 200, ct);
            }
            else
            {
                await SendAsync(new PaymentResponse { Status = "error", ErrorMessage = "Ödeme işlemi başarısız oldu." }, 400, ct);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ödeme işlemi sırasında bir hata oluştu");
            await SendAsync(new PaymentResponse
            {
                Status = "error",
                ErrorMessage = "Ödeme işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyin."
            }, 500, ct);
        }
    }


    private PaymentRequestModel CreatePaymentRequestModel(PaymentViewModel paymentViewModel, AccountViewModel accountInfo)
    {
        return new PaymentRequestModel
        {
            Price = Convert.ToDecimal(paymentViewModel.Price),
            PaidPrice = Convert.ToDecimal(paymentViewModel.Price),
            Card = new CardModel
            {
                CardHolderName = paymentViewModel.CardHolderName,
                CardNumber = paymentViewModel.CardNumber,
                ExpireMonth = paymentViewModel.ExpireMonth,
                ExpireYear = paymentViewModel.ExpireYear,
                Cvc = paymentViewModel.Cvc
            },
            Buyer = new BuyerModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = accountInfo.Name,
                Surname = accountInfo.Surname,
                GsmNumber = accountInfo.GsmNumber,
                Email = accountInfo.Email,
                IdentityNumber = accountInfo.IdentityNumber,
                LastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                RegistrationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                RegistrationAddress = accountInfo.RegistrationAddress,
                Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                City = accountInfo.City,
                Country = accountInfo.Country,
                ZipCode = accountInfo.ZipCode
            },
            BasketItems = new List<BasketItemModel>
        {
                new BasketItemModel
                    {
                        Id = "BI" + DateTime.Now.Ticks,
                        Name = "Örnek Ürün",
                        Category1 = "Kategori 1",
                        Category2 = "Kategori 2", // Bu satırı ekleyin
                        ItemType = "PHYSICAL",
                        Price = Convert.ToDecimal(paymentViewModel.Price)
                    }
        }
        };
    }


    private async Task<PaymentResponse> SendPaymentRequest(PaymentRequestModel requestModel)
    {
        var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/payment/initiate", content);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"API Raw Response: {result}");
            try
            {
                return JsonConvert.DeserializeObject<PaymentResponse>(result);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, $"JSON Deserialization hatası. Raw content: {result}");
                return new PaymentResponse { Status = "error", ErrorMessage = "API yanıtı işlenemedi." };
            }
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError($"API isteği başarısız oldu. Durum Kodu: {(int)response.StatusCode}, Neden: {response.ReasonPhrase}, İçerik: {errorContent}");
            return new PaymentResponse { Status = "error", ErrorMessage = "API isteği başarısız oldu." };
        }
    }

    private AccountViewModel GetAccountInfo()
    {
        var accountInfoJson = HttpContext.Session.GetString("AccountInfo");
        return string.IsNullOrEmpty(accountInfoJson) ? null : JsonConvert.DeserializeObject<AccountViewModel>(accountInfoJson);
    }
}