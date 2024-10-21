using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class OrderHistory
    {
        public long Id { get; set; }
        public long OrderInformationId { get; set; }
        public long StateId { get; set; }
        public DateTime StatusUpdatedTime { get; set; }

        public OrderInformations OrderInformation { get; set; }
        public OrderState State { get; set; }
    }
}
