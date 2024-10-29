namespace ECommerceView.Endpoints.Baskets
{
    using FastEndpoints;
    using Microsoft.AspNetCore.Http;
    using global::ECommerceView.Models;
    using Newtonsoft.Json;

    namespace ECommerceView.Endpoints.Baskets
    {
        public class RemoveFromCartEndpoint : Endpoint<RemoveFromCartRequestViewModel>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;

            public RemoveFromCartEndpoint(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }

            public override void Configure()
            {
                Delete("api/cart/remove"); // Endpoint URL'si
                AllowAnonymous();
            }

            public override async Task HandleAsync(RemoveFromCartRequestViewModel req, CancellationToken ct)
            {
                // Cookie'den mevcut sepeti al
                var cart = GetCartFromCookie();

                // Ürünü sepetten kaldır
                var item = cart.Find(i => i.productId == req.ProductId);
                if (item != null)
                {
                    cart.Remove(item);
                    SaveCartToCookie(cart); // Güncellenmiş sepeti cookie'ye kaydet
                }

                await SendOkAsync();
            }

            private List<CartItemViewModel> GetCartFromCookie()
            {
                var cart = _httpContextAccessor.HttpContext.Request.Cookies["cart"];
                return string.IsNullOrEmpty(cart) ? new List<CartItemViewModel>() : JsonConvert.DeserializeObject<List<CartItemViewModel>>(cart);
            }

            private void SaveCartToCookie(List<CartItemViewModel> cart)
            {
                var cookieValue = JsonConvert.SerializeObject(cart);
                _httpContextAccessor.HttpContext.Response.Cookies.Append("cart", cookieValue, new CookieOptions
                {
                    Path = "/",
                    MaxAge = TimeSpan.FromDays(30) // Cookie süresi
                });
            }
        }

        public class RemoveFromCartRequestViewModel
        {
            public int ProductId { get; set; }
        }
    }
}
