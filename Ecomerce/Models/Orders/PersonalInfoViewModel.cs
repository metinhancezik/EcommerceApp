using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerceView.Models.Orders
{
    public class PersonalInfoViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string IdentityNumber { get; set; }
        public string Address { get; set; }

        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public int NeighborhoodId { get; set; }

        public List<SelectListItem> Cities { get; set; }
        public List<SelectListItem> Districts { get; set; }
        public List<SelectListItem> Neighborhoods { get; set; }
    }
}
