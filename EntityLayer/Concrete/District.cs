using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class District
    {
        public int Id { get; set; }
        public string DistrictName { get; set; }
        public int CityId { get; set; }

        public City City { get; set; }
        public ICollection<OrderInformations> OrderInformations { get; set; }
        public ICollection<Neighborhood> Neighborhoods { get; set; }
    }
}
