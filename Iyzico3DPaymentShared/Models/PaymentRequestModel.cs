using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Iyzico3DPayment.Shared.Models
{
    public class PaymentRequestModel
    {
        public string Price { get; set; }
        public string PaidPrice { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpireMonth { get; set; }
        public string ExpireYear { get; set; }
        public string Cvc { get; set; }
    }
}
