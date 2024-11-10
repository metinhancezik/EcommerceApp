namespace ECommerceView.Models.Cart
{
    public class UpdateQuantityResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public decimal NewTotal { get; set; }
        public int NewQuantity { get; set; }
    }
}
