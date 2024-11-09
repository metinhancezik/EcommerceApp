using FastEndpoints;
using ECommerceView.Models;
using Iyzico3DPaymentShared.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Iyzico3DPayment.Shared.Models;
using ECommerceView.Models.Payment;
using ECommerceView.Models.Account;
using ServiceLayer.Abstract;

namespace ECommerceView.Endpoints.Payment;

public class ProcessPaymentEndpoint : Endpoint<PaymentViewModel, PaymentResponse>
{
    private readonly ILogger<ProcessPaymentEndpoint> _logger;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IAuthTokensService _authTokenService;
    private readonly IProductsService _productService;
    private readonly IOrderInformationsService _orderInformationsService;
    private readonly ICartService _cartService;
    private readonly ICartItemsService _cartItemsService;
    public ProcessPaymentEndpoint(ILogger<ProcessPaymentEndpoint> logger, 
        IHttpClientFactory httpClientFactory, IConfiguration configuration, IAuthTokensService authTokensService,
        IProductsService productService, IOrderInformationsService orderInformationsService, 
        ICartService cartService,ICartItemsService cartItemsService)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient();
        _configuration = configuration;
        _httpClient.BaseAddress = new Uri(_configuration["ApiSettings:BaseUrl"]);
        _authTokenService = authTokensService;
        _productService = productService;
        _orderInformationsService = orderInformationsService;
        _cartService = cartService;
        _cartItemsService = cartItemsService;
    }

    public override void Configure()
    {
        Post("/payment/process");
    }

    public override async Task HandleAsync(PaymentViewModel req, CancellationToken ct)
    {
        try
        {
            _logger.LogInformation("Ödeme işlemi başlatıldı: {@PaymentDetails}", req); 

            var accountInfo = await GetAccountInfo();
            if (accountInfo == null)
            {
                _logger.LogWarning("Kullanıcı bilgileri bulunamadı");
                await SendAsync(new PaymentResponse { Status = "error", ErrorMessage = "Kullanıcı bilgileri bulunamadı." }, 400, ct);
                return;
            }

            var requestModel = await CreatePaymentRequestModel(req, accountInfo);
            var paymentResponse = await SendPaymentRequest(requestModel);

            if (paymentResponse?.Status == "success") 
            {
                await SendAsync(paymentResponse, 200, ct);
            }
            else
            {
                var errorMessage = paymentResponse?.ErrorMessage ?? "Ödeme işlemi başarısız oldu.";
                await SendAsync(new PaymentResponse { Status = "error", ErrorMessage = errorMessage }, 400, ct);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ödeme işlemi sırasında bir hata oluştu: {ErrorMessage}", ex.Message);
            await SendAsync(new PaymentResponse
            {
                Status = "error",
                ErrorMessage = "Ödeme işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyin."
            }, 500, ct);
        }
    }
    private async Task<PaymentRequestModel> CreatePaymentRequestModel(PaymentViewModel paymentViewModel, AccountViewModel accountInfo)
    {
        var token = User.Claims.FirstOrDefault(c => c.Type == "token")?.Value;
        var userId = await _authTokenService.GetUserIdFromTokenAsync(token);
        if (!userId.HasValue)
        {
            throw new Exception("Kullanıcı ID'si bulunamadı");
        }
        var cart = await _cartService.GetByUserId(userId.Value);
        if (cart == null)
        {
            throw new Exception("Sepet bulunamadı");
        }

        var cartItems = await _cartItemsService.GetCartItemsByCartId(cart.Id);
        decimal totalPrice = cartItems.Sum(x => x.TotalPrice);

        var basketItems = new List<BasketItemModel>();

        foreach (var item in cartItems)
        {
            var product = _productService.GetProductByLongId(item.ProductId);
            var itemTotalPrice = item.UnitPrice * item.Quantity; 
        

            basketItems.Add(new BasketItemModel
            {
                Id = item.Id.ToString(),
                Name = product?.ProductName ?? "Bilinmeyen Ürün",
                Category1 = "Genel",
                Category2 = "Genel 2",
                ItemType = "PHYSICAL",
                Price = itemTotalPrice,
                Quantity = item.Quantity.ToString()
            });
        }

        return new PaymentRequestModel
        {
            Price = totalPrice,
            PaidPrice = totalPrice,
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
                Id = userId.Value.ToString(),
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
            BasketItems = basketItems
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
                return new PaymentResponse
                {
                    Status = "success",
                    ErrorMessage = null
                };
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

    private async Task<AccountViewModel> GetAccountInfo()
    {
        try
        {
            var token = User.Claims.FirstOrDefault(c => c.Type == "token")?.Value;
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Token bulunamadı");
                return null;
            }

            var userId = await _authTokenService.GetUserIdFromTokenAsync(token);
            if (!userId.HasValue)
            {
                _logger.LogWarning("UserId bulunamadı");
                return null;
            }

            var orderInfo = await _orderInformationsService.GetLastOrderByUserId(userId.Value);
            if (orderInfo == null)
            {
                _logger.LogWarning("Sipariş bilgileri bulunamadı");
                return null;
            }
            return new AccountViewModel
            {
                Name = orderInfo.Name,
                Surname = orderInfo.Surname,
                GsmNumber = orderInfo.Phone,
                Email = User.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? "",
                IdentityNumber = orderInfo.IdentityNumber,
                RegistrationAddress = orderInfo.Address,
                City = orderInfo.City?.CityName ?? "",
                Country = orderInfo.Country?.CountryName ?? "Türkiye",
                ZipCode = "34000"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı bilgileri alınırken hata oluştu");
            return null;
        }
    }
}