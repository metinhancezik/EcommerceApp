using FastEndpoints;
using ECommerceView.Models; // CartItemViewModel sınıfının bulunduğu namespace
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ECommerceView.Endpoints.Baskets
{
    public class AddToCartEndpoint : Endpoint<CartItemViewModel>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddToCartEndpoint(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override void Configure()
        {
            Post("api/cart/add"); 
            AllowAnonymous();
        }

        public override async Task HandleAsync(CartItemViewModel req, CancellationToken ct)
        {
            
            var cart = GetCartFromCookie();

            
            var item = cart.Find(i => i.productId == req.productId);
            if (item != null)
            {
                item.quantity += req.quantity; 
            }
            else
            {
                cart.Add(new CartItemViewModel
                {
                    productId = req.productId,
                    productName=req.productName,
                    quantity = req.quantity,
                    price=req.price,
                });
            }

            
            SaveCartToCookie(cart);

            
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                // Veritabanına güncelleme yap
                // await _cartRepository.UpdateCartAsync(cart);
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
                MaxAge = TimeSpan.FromDays(30) 
            });
        }
    }
}