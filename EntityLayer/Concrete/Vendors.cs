using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Vendors
    {
        public long Id { get; set; }
        public string VendorName { get; set; }
        public string ContactPerson { get; set; }
        public string VendorPhone { get; set; }
        public string VendorEmail { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsActive { get; set; }

        public ICollection<OrderStatus> OrderStatuses { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; }
        public ICollection<CartItems> CartItems { get; set; }
        public ICollection<Products> Products { get; set; }
    }
}
