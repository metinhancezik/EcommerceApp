namespace ECommerceView.Models.Cart
{
    public class CartItemViewModel
    {
        public int productId { get; set; }
        public string productName { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
    }
}
