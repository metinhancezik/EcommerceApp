using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iyzico3DPayment.Shared.Models
{
    public class BasketItemModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category1 { get; set; }
        public string Category2 { get; set; } 
        public string ItemType { get; set; }
        public string Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
