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

namespace Iyzico3DPaymentProject.Controllers
{
    public class PaymentViewController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentViewController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public PaymentViewController(IConfiguration configuration, ILogger<PaymentViewController> logger, IHttpClientFactory clientFactory)
        {
            _configuration = configuration;
            _logger = logger;
            _clientFactory = clientFactory;
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

                if (paymentResponse != null)
                {
                    return Content(paymentResponse.HtmlContent, "text/html");
                }
                else
                {
                    return View("Error", new ErrorViewModel { Message = "Ödeme işlemi başarısız oldu." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ödeme işlemi sırasında bir hata oluştu: {ex.Message}");
                return View("Error", new ErrorViewModel { Message = "Bir hata oluştu. Lütfen daha sonra tekrar deneyin." });
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

            // API'ye callback bilgilerini ilet
            var client = _clientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"];
            var content = new FormUrlEncodedContent(form.ToDictionary(x => x.Key, x => x.Value.ToString()));
            var response = await client.PostAsync($"{apiBaseUrl}/api/payment/callback", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                // Başarılı ödeme sayfasına yönlendir
                return RedirectToAction("SuccessfulPayment", "Pages");
            }
            else
            {
                // Hata sayfasına yönlendir
                return RedirectToAction("FailedPayment", "Pages");
            }
        }

        private AccountViewModel GetAccountInfo()
        {
            var accountInfoJson = HttpContext.Session.GetString("AccountInfo");
            return string.IsNullOrEmpty(accountInfoJson) ? null : JsonConvert.DeserializeObject<AccountViewModel>(accountInfoJson);
        }

        private PaymentRequestModel CreatePaymentRequestModel(PaymentViewModel paymentViewModel, AccountViewModel accountInfo)
        {
            return new PaymentRequestModel
            {
                Price = Convert.ToInt32(paymentViewModel.Price),
                PaidPrice = Convert.ToInt32(paymentViewModel.Price),
                CardHolderName = paymentViewModel.CardHolderName,
                CardNumber = paymentViewModel.CardNumber,
                ExpireMonth = paymentViewModel.ExpireMonth,
                ExpireYear = paymentViewModel.ExpireYear,
                Cvc = paymentViewModel.Cvc,
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
                }
            };
        }

        private async Task<PaymentResponse> SendPaymentRequest(PaymentRequestModel requestModel)
        {
            var client = _clientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"];
            client.BaseAddress = new Uri(apiBaseUrl);

            var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/payment/initiate", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PaymentResponse>(result);
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"API isteği başarısız oldu. Durum Kodu: {(int)response.StatusCode}, Neden: {response.ReasonPhrase}, İçerik: {errorContent}");
                return null;
            }
        }
    }
}