using Iyzico3DPaymentAPI.Interfaces;
using Iyzico3DPayment.Shared.Models;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Iyzico3DPaymentAPI.Services
{
    public class IyzicoPaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly string _callbackUrl;

        public IyzicoPaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
            _callbackUrl = _configuration["PaymentSettings:CallbackUrl"];
        }

        public async Task<ThreedsInitialize> InitiatePayment(PaymentRequestModel model)
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
                CardHolderName = model.Card.CardHolderName,
                CardNumber = model.Card.CardNumber,
                ExpireMonth = model.Card.ExpireMonth,
                ExpireYear = model.Card.ExpireYear,
                Cvc = model.Card.Cvc,
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
                Ip = model.Buyer.Ip
            };

            request.Buyer = buyer;
            request.PaymentCard = paymentCard;

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

            request.BasketItems = model.BasketItems.Select(item => new BasketItem
            {
                Id = item.Id,
                Name = item.Name,
                Category1 = item.Category1,
                Category2 = item.Category2,
                ItemType = item.ItemType,
                Price = item.Price.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            return await Task.FromResult(ThreedsInitialize.Create(request, options));
        }

        public async Task<ThreedsPayment> ProcessCallback(IDictionary<string, string> callbackData)
        {
            CreateThreedsPaymentRequest request = new CreateThreedsPaymentRequest
            {
                ConversationId = callbackData["conversationId"],
                PaymentId = callbackData["paymentId"],
                ConversationData = callbackData["conversationData"]
            };

            Options options = new Options
            {
                ApiKey = _configuration["IyzicoSettings:ApiKey"],
                SecretKey = _configuration["IyzicoSettings:SecretKey"],
                BaseUrl = _configuration["IyzicoSettings:BaseUrl"]
            };

            return await Task.FromResult(ThreedsPayment.Create(request, options));
        }
    }
}