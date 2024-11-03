namespace ECommerceView.Models.Cart
{
    public class SyncCartRequestModel
    {
        public List<CartCookieItemModel> CartItems { get; set; }

        public SyncCartRequestModel()
        {
            CartItems = new List<CartCookieItemModel>();
        }
    }
}
