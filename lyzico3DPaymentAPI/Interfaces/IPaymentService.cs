using Iyzico3DPayment.Shared.Models;
using Iyzipay.Model;

namespace Iyzico3DPaymentAPI.Interfaces
{
    public interface IPaymentService
    {
        Task<ThreedsInitialize> InitiatePayment(PaymentRequestModel model);
        Task<ThreedsPayment> ProcessCallback(IDictionary<string, string> callbackData);
    }
}