using Iyzico3DPaymentShared.Models;
using System;

namespace Iyzico3DPayment.Shared.Models
{
    public class PaymentRequestModel
    {
        public decimal Price { get; set; }
        public decimal PaidPrice { get; set; }
        public CardModel Card { get; set; }
        public BuyerModel Buyer { get; set; }
        public List<BasketItemModel> BasketItems { get; set; }
    }
}   