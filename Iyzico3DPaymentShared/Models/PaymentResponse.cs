using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Iyzico3DPaymentShared.Models
{
    public class PaymentResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonProperty("htmlContent")]
        public string HtmlContent { get; set; }

        // API'den dönen diğer alanları da ekleyin
    }
}
