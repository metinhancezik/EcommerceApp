namespace ECommerceView.Models.Cart
{
    public class UpdateQuantityRequest
    {
        public int ProductId { get; set; }
        public int Change { get; set; }  
    }
}
