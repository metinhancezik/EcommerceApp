using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class OrderStatus
    {
        public long Id { get; set; }
        public long OrderInformationId { get; set; }
        public bool PaymentStatus { get; set; }
        public long StateId { get; set; }
        public long VendorId { get; set; }
        public DateTime UpdatedTime { get; set; }

        public OrderInformations OrderInformation { get; set; }
        public OrderState State { get; set; }
        public Vendors Vendor { get; set; }
    }
}
