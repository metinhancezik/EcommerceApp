using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class OrderInformations
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public int NeighborhoodId { get; set; }
        public string Address { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime CreatedTime { get; set; }

        public UserDetails User { get; set; }
        public Country Country { get; set; }
        public City City { get; set; }
        public District District { get; set; }
        public Neighborhood Neighborhood { get; set; }
        public ICollection<OrderStatus> OrderStatuses { get; set; }
        public ICollection<OrderHistory> OrderHistories { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; }
    }
}
