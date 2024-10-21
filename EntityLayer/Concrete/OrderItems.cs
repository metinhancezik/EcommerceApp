using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class OrderItems
    {
        public long Id { get; set; }
        public long OrderInformationId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public long VendorId { get; set; }

        public OrderInformations OrderInformation { get; set; }
        public Products Product { get; set; }
        public Vendors Vendor { get; set; }
    }
}
