using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class City
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public int CountryId { get; set; }

        public Country Country { get; set; }
        public ICollection<OrderInformations> OrderInformations { get; set; }
        public ICollection<District> Districts { get; set; }
    }
}
