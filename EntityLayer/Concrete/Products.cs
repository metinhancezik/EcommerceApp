using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Products
    {
        public long Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal UnitPrice { get; set; }
        public int Stock { get; set; }
        public long VendorId { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }

        public Vendors Vendor { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; }
        public ICollection<CartItems> CartItems { get; set; }
    }
}
