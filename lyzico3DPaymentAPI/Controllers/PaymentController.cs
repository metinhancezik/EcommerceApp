using Iyzico3DPayment.Shared.Models;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Globalization;

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

            var request = new CreatePaymentRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = Guid.NewGuid().ToString(),
                Price = model.Price.ToString(CultureInfo.InvariantCulture),
                PaidPrice = model.PaidPrice.ToString(CultureInfo.InvariantCulture),
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

            var buyer = new Buyer
            {
                Id = model.Buyer.Id,
                Name = model.Buyer.Name,
                Surname = model.Buyer.Surname,
                IdentityNumber = model.Buyer.IdentityNumber,
                Email = model.Buyer.Email,
                GsmNumber = model.Buyer.GsmNumber,
                RegistrationDate = model.Buyer.RegistrationDate,
                LastLoginDate = model.Buyer.LastLoginDate,
                RegistrationAddress = model.Buyer.RegistrationAddress,
                City = model.Buyer.City,
                Country = model.Buyer.Country,
                ZipCode = model.Buyer.ZipCode,
                Ip = model.Buyer.Ip ?? HttpContext.Connection.RemoteIpAddress?.ToString()
            };

            request.Buyer = buyer;
            request.PaymentCard = paymentCard;

            // Adres bilgilerini ekleyin
            request.ShippingAddress = new Address
            {
                ContactName = $"{buyer.Name} {buyer.Surname}",
                City = buyer.City,
                Country = buyer.Country,
                Description = buyer.RegistrationAddress,
                ZipCode = buyer.ZipCode
            };

            request.BillingAddress = new Address
            {
                ContactName = $"{buyer.Name} {buyer.Surname}",
                City = buyer.City,
                Country = buyer.Country,
                Description = buyer.RegistrationAddress,
                ZipCode = buyer.ZipCode
            };

            // Sepet öğelerini ekleyin (örnek olarak)
            request.BasketItems = new List<BasketItem>
            {
                new BasketItem
                {
                    Id = "BI" + DateTime.Now.Ticks,
                    Name = "Örnek Ürün",
                    Category1 = "Kategori",
                    ItemType = BasketItemType.PHYSICAL.ToString(),
                    Price = model.Price.ToString(CultureInfo.InvariantCulture)
                }
            };

            var threedsInitialize = ThreedsInitialize.Create(request, options);

            if (threedsInitialize.Status == "success")
            {
                return Ok(new { HtmlContent = threedsInitialize.HtmlContent });
            }
            else
            {
                return BadRequest(new { ErrorMessage = threedsInitialize.ErrorMessage });
            }
        }

        [HttpGet("callback")]
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
                ApiKey = _configuration["IyzicoSettings:ApiKey"],
                SecretKey = _configuration["IyzicoSettings:SecretKey"],
                BaseUrl = _configuration["IyzicoSettings:BaseUrl"]
            };

            ThreedsPayment threedsPayment = ThreedsPayment.Create(request, options);

            if (threedsPayment.Status == "success")
            {
                // Ödeme başarılı, veritabanınızı güncelleyin ve kullanıcıyı bilgilendirin
                return RedirectToAction("SuccessfulPayment", "Pages");
            }
            else
            {
                // Ödeme başarısız, kullanıcıyı bilgilendirin
                return BadRequest(new { ErrorMessage = threedsPayment.ErrorMessage });
            }
        }
    }
}