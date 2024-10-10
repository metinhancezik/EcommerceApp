using Iyzico3DPayment.Shared.Models;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Iyzico3DPaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _callbackUrl;

        public PaymentController(IConfiguration configuration)
        {
           
            _configuration = configuration;
            _callbackUrl = _configuration["PaymentSettings:CallbackUrl"];

            Console.WriteLine($"ApiKey: {_configuration["IyzicoSettings:ApiKey"]}");
            Console.WriteLine($"SecretKey: {_configuration["IyzicoSettings:SecretKey"]}");
            Console.WriteLine($"BaseUrl: {_configuration["IyzicoSettings:BaseUrl"]}");
            Console.WriteLine($"CallbackUrl: {_callbackUrl}");
        }

        [HttpPost("initiate")]
        public IActionResult InitiatePayment([FromBody] PaymentRequestModel model)
        {
            var options = new Options
            {
                ApiKey = _configuration["IyzicoSettings:ApiKey"],
                SecretKey = _configuration["IyzicoSettings:SecretKey"],
                BaseUrl = _configuration["IyzicoSettings:BaseUrl"]
            };
            Console.WriteLine($"Options - ApiKey: {options.ApiKey}");
            Console.WriteLine($"Options - SecretKey: {options.SecretKey}");
            Console.WriteLine($"Options - BaseUrl: {options.BaseUrl}");
            var request = new CreatePaymentRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = Guid.NewGuid().ToString(),
                Price = model.Price,
                PaidPrice = model.PaidPrice,
                Currency = Currency.TRY.ToString(),
                Installment = 1,
                BasketId = "B" + DateTime.Now.Ticks,
                PaymentChannel = PaymentChannel.WEB.ToString(),
                PaymentGroup = PaymentGroup.PRODUCT.ToString(),
                CallbackUrl = _callbackUrl
            };

            var paymentCard = new PaymentCard
            {
                CardHolderName = model.CardHolderName,
                CardNumber = model.CardNumber,
                ExpireMonth = model.ExpireMonth,
                ExpireYear = model.ExpireYear,
                Cvc = model.Cvc,
                RegisterCard = 0
            };
            request.PaymentCard = paymentCard;

            // Buyer, Address ve BasketItems bilgilerini de ekleyin

            var threedsInitialize = ThreedsInitialize.Create(request, options); // Options Kısmında null

            if (threedsInitialize.Status == "success")
            {
                return Ok(new { HtmlContent = threedsInitialize.HtmlContent });
            }
            else
            {
                return BadRequest(new { ErrorMessage = threedsInitialize.ErrorMessage });
            }
        }


        [HttpPost("callback")]
        public IActionResult Callback([FromForm] IFormCollection form)
        {
            string conversationId = form["conversationId"];
            string paymentId = form["paymentId"];
            string conversationData = form["conversationData"];

            CreateThreedsPaymentRequest request = new CreateThreedsPaymentRequest
            {
                ConversationId = conversationId,
                PaymentId = paymentId,
                ConversationData = conversationData
            };

            Options options = new Options
            {
                ApiKey = "sandbox-xFgd2zfpxGx07FE4oEuvWj8aRLb97RIO",
                SecretKey = "sandbox-qgMiTKPIMS6bFpZwLjTPipDQw58q7IfV",
                BaseUrl = "https://sandbox-api.iyzipay.com"
            };

            ThreedsPayment threedsPayment = ThreedsPayment.Create(request, options);

            if (threedsPayment.Status == "success")
            {
                // Ödeme başarılı, veritabanınızı güncelleyin ve kullanıcıyı bilgilendirin
                return Ok(new { Message = "Ödeme başarılı", PaymentId = threedsPayment.PaymentId });
            }
            else
            {
                // Ödeme başarısız, kullanıcıyı bilgilendirin
                return BadRequest(new { ErrorMessage = threedsPayment.ErrorMessage });
            }
        }
    }


}