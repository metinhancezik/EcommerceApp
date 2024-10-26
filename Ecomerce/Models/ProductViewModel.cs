namespace ECommerceView.Models
{
    public class ProductViewModel
    {
        public long Id { get; set; }
        public string ProductName { get; set; } 
        public string ProductDescription { get; set; } 
        public decimal UnitPrice { get; set; } 
        public int Stock { get; set; }
        public long VendorId { get; set; } 
        public string VendorName { get; set; } 
        public string ImageUrl { get; set; }
    }
}