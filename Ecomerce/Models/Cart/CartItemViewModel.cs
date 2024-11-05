namespace ECommerceView.Models.Cart
{
    public class CartItemViewModel
    {
        public long productId { get; set; }
        public string productName { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
        public string imageUrl { get; set; }
    }
}
