using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class OrderState
    {
        public long Id { get; set; }
        public string OrderStateDescription { get; set; }

        public ICollection<OrderStatus> OrderStatuses { get; set; }
        public ICollection<OrderHistory> OrderHistories { get; set; }
    }
}
