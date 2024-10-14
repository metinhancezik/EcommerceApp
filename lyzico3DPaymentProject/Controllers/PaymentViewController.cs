using Iyzico3DPaymentShared.Models;
using Iyzico3DPaymentProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Iyzico3DPayment.Shared.Models;
using lyzico3DPaymentProject.Models;
using System.Web;
using ECommerceView.Models;

namespace Iyzico3DPaymentProject.Controllers
{
    public class PaymentViewController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentViewController> _logger;
        private readonly HttpClient _httpClient;

        public PaymentViewController(IConfiguration configuration, ILogger<PaymentViewController> logger, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(_configuration["ApiSettings:BaseUrl"]);
        }

        public IActionResult Index()
        {
            var accountInfo = HttpContext.Session.GetString("AccountInfo");
            if (string.IsNullOrEmpty(accountInfo))
            {
                return RedirectToAction("Account", "Pages");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(PaymentViewModel paymentViewModel)
        {
            try
            {
                var accountInfo = GetAccountInfo();
                if (accountInfo == null)
                {
                    return RedirectToAction("Account", "Pages");
                }

                var requestModel = CreatePaymentRequestModel(paymentViewModel, accountInfo);
                var paymentResponse = await SendPaymentRequest(requestModel);
                _logger.LogInformation($"API Response: {JsonConvert.SerializeObject(paymentResponse)}");

                if (paymentResponse != null)
                {
                    return Content(paymentResponse.HtmlContent, "text/html");
                }
                else
                {
                    return View("Error", new ErrorViewModel { Message = paymentResponse?.ErrorMessage ?? "Ödeme işlemi başarısız oldu." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ödeme işlemi sırasında bir hata oluştu");
                return View("Error", new ErrorViewModel { Message = "Bir hata oluştu. Lütfen daha sonra tekrar deneyin." });
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
                _logger.LogInformation($"API Raw Response: {result}"); // Bu satırı ekleyin
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
        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> Callback()
        {
            _logger.LogInformation("MVC Callback method called");

            var form = await Request.ReadFormAsync();
            foreach (var key in form.Keys)
            {
                _logger.LogInformation($"{key}: {form[key]}");
            }

            var content = new FormUrlEncodedContent(form.ToDictionary(x => x.Key, x => x.Value.ToString()));
            var response = await _httpClient.PostAsync("api/payment/callback", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Callback API Response: {result}");
                var paymentResult = JsonConvert.DeserializeObject<dynamic>(result);

                if (paymentResult.message == "Ödeme başarılı")
                {
                    _logger.LogInformation("Payment successful");
                    TempData["PaymentStatus"] = "success";
                    TempData["PaymentMessage"] = "Ödemeniz başarıyla gerçekleştirildi.";
                }
                else
                {
                    _logger.LogWarning($"Payment not successful. Message: {paymentResult.message}");
                    TempData["PaymentStatus"] = "error";
                    TempData["PaymentMessage"] = "Ödemeniz başarısız oldu.";
                }
            }
            else
            {
                _logger.LogWarning($"API request failed. Status code: {response.StatusCode}");
                TempData["PaymentStatus"] = "error";
                TempData["PaymentMessage"] = "Ödeme işlemi sırasında bir hata oluştu.";
            }

            return RedirectToAction("Home", "Pages");
        }

        private AccountViewModel GetAccountInfo()
        {
            var accountInfoJson = HttpContext.Session.GetString("AccountInfo");
            return string.IsNullOrEmpty(accountInfoJson) ? null : JsonConvert.DeserializeObject<AccountViewModel>(accountInfoJson);
        }
    }
}