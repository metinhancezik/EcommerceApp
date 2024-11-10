using EntityLayer.Concrete;

namespace ECommerceView.Models.Orders
{
    public class CompleteOrderRequest
    {
        public long UserId { get; set; }
        public string BuyerName { get; set; }
        public string BuyerSurname { get; set; }
        public string BuyerPhone { get; set; }
        public string BuyerAddress { get; set; }
        public string BuyerIdentityNumber { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public int NeighborhoodId { get; set; }
        public int CountryId { get; set; }
        public List<OrderItems> Items { get; set; }
    }
}
