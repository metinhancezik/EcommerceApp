namespace ECommerceView.Endpoints.Baskets
{
    using FastEndpoints;
    using Microsoft.AspNetCore.Http;
    using ECommerceView.Models.Cart;
    using Newtonsoft.Json;
    using ServiceLayer.Abstract;
    using ECommerceView.Endpoints.Interfaces;

    public class RemoveFromCartEndpoint : Endpoint<RemoveFromCartRequestViewModel>, IRemoveFromCartEndpoint
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICartService _cartService;
        private readonly ICartItemsService _cartItemsService;
        private readonly IAuthTokensService _authTokensService;

        public RemoveFromCartEndpoint(
            IHttpContextAccessor httpContextAccessor,
            ICartService cartService,
            ICartItemsService cartItemsService,
            IAuthTokensService authTokensService)
        {
            _httpContextAccessor = httpContextAccessor;
            _cartService = cartService;
            _cartItemsService = cartItemsService;
            _authTokensService = authTokensService;
        }

        public override void Configure()
        {
            Delete("api/cart/remove");
            AllowAnonymous();
        }

        public override async Task HandleAsync(RemoveFromCartRequestViewModel req, CancellationToken ct)
        {
            try
            {
                // Token kontrolü
                var token = HttpContext.Request.Cookies["auth_token"];
                var userId = await _authTokensService.GetUserIdFromTokenAsync(token);

                if (userId.HasValue) // Kullanıcı login olmuşsa
                {
                    // Database'den sepeti al
                    var cart = await _cartService.GetByUserId(userId.Value);
                    if (cart != null)
                    {
                        // Sepetteki ürünü bul ve sil
                        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == req.ProductId);
                        if (cartItem != null)
                        {
                            _cartItemsService.TDelete(cartItem);
                        }
                    }
                }

                // Cookie işlemleri (hem login olan hem olmayan için)
                var cookieCart = GetCartFromCookie();
                var item = cookieCart.Find(i => i.productId == req.ProductId);
                if (item != null)
                {
                    cookieCart.Remove(item);
                    SaveCartToCookie(cookieCart);
                }

                await SendAsync(new
                {
                    success = true,
                    message = "Ürün sepetten kaldırıldı",
                    cartItemCount = cookieCart.Count
                }, 200, ct);
            }
            catch (Exception ex)
            {
                await SendAsync(new
                {
                    success = false,
                    message = "Ürün sepetten kaldırılırken bir hata oluştu"
                }, 500, ct);
            }
        }

        private List<CartItemViewModel> GetCartFromCookie()
        {
            var cart = _httpContextAccessor.HttpContext.Request.Cookies["cart"];
            return string.IsNullOrEmpty(cart)
                ? new List<CartItemViewModel>()
                : JsonConvert.DeserializeObject<List<CartItemViewModel>>(cart);
        }

        private void SaveCartToCookie(List<CartItemViewModel> cart)
        {
            if (cart.Any())
            {
                var cookieValue = JsonConvert.SerializeObject(cart);
                _httpContextAccessor.HttpContext.Response.Cookies.Append("cart",
                    cookieValue,
                    new CookieOptions
                    {
                        Path = "/",
                        HttpOnly = false,
                        MaxAge = TimeSpan.FromDays(30),
                        SameSite = SameSiteMode.Lax
                    });
            }
            else
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Delete("cart");
            }
        }
    }

    public class RemoveFromCartRequestViewModel
    {
        public long ProductId { get; set; } // int yerine long kullanıyoruz
    }
}