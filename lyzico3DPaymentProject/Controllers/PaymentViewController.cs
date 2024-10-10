using Iyzico3DPayment.Shared.Models;
using Iyzico3DPaymentProject.Models;
using Iyzico3DPaymentShared.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Iyzico3DPaymentProject.Controllers
{

    public class PaymentViewController : Controller
    {
        private readonly IConfiguration _configuration;

        public PaymentViewController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(PaymentViewModel viewModel)
        {
            var requestModel = new PaymentRequestModel
            {
                Price = viewModel.Price,
                PaidPrice = viewModel.Price, 
                CardHolderName = viewModel.CardHolderName,
                CardNumber = viewModel.CardNumber,
                ExpireMonth = viewModel.ExpireMonth,
                ExpireYear = viewModel.ExpireYear,
                Cvc = viewModel.Cvc
            };

            using (var client = new HttpClient())
            {
                var apiBaseUrl = _configuration["ApiSettings:BaseUrl"];
                client.BaseAddress = new Uri(apiBaseUrl);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/payment/initiate", content);




                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var paymentResponse = JsonConvert.DeserializeObject<PaymentResponse>(result);

                    return Content(paymentResponse.HtmlContent, "text/html");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var statusCode = (int)response.StatusCode;
                    var reasonPhrase = response.ReasonPhrase;

                    // Hata detaylarını loglayın
                    Console.WriteLine($"API isteği başarısız oldu. Durum Kodu: {statusCode}, Neden: {reasonPhrase}, İçerik: {errorContent}");

                    return Ok();
                }
            
            }
        }

        public IActionResult Callback()
        {
            // 3D Secure işlemi tamamlandıktan sonra kullanıcıyı yönlendireceğiniz sayfa
            return View();
        }
    }
}