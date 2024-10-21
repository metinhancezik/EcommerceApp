using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Neighborhood
    {
        public int Id { get; set; }
        public string NeighborhoodName { get; set; }
        public int DistrictId { get; set; }

        public District District { get; set; }
        public ICollection<OrderInformations> OrderInformations { get; set; }
    }
}
