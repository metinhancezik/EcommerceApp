using AuthenticationLayer.Interfaces;
using ECommerceView.Endpoints.Interfaces;
using ECommerceView.Models.Cart;
using EntityLayer.Concrete;
using FastEndpoints;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Newtonsoft.Json;
using ServiceLayer.Abstract;

public class SyncCartToDatabaseEndpoint : Endpoint<SyncCartRequestModel>, ISyncCartToDatabaseEndpoint
{
    private readonly ICartService _cartService;
    private readonly IAuthService _authService;
    private readonly IAuthTokensService _authTokensService;
    private readonly ICartItemsService _cartItemsService;
    private readonly IProductsService _productService;
    private readonly IHttpContextAccessor _httpContextAccessor;  

    public SyncCartToDatabaseEndpoint(
        ICartService cartService,
        IAuthService authService,
        IAuthTokensService authTokensService,
        ICartItemsService cartItemsService,
        IProductsService productService,
        IHttpContextAccessor httpContextAccessor)
    {
        _cartService = cartService;
        _authService = authService;
        _authTokensService = authTokensService;
        _cartItemsService = cartItemsService;
        _productService = productService;
        _httpContextAccessor = httpContextAccessor; 
    }

    public override void Configure()
    {
        Post("/api/basket/sync");
    }

    public override async Task HandleAsync(SyncCartRequestModel req, CancellationToken ct)
    {
        try
        {

            var token = _httpContextAccessor.HttpContext?.Request.Cookies["auth_token"];
            if (string.IsNullOrEmpty(token))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            // Token geçerliliğini kontrol et
            if (!await _authService.ValidateTokenAsync(token))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            // UserId'yi al
            var userId = await _authTokensService.GetUserIdFromTokenAsync(token);
            if (!userId.HasValue)
            {
                await SendAsync(new
                {
                    success = false,
                    message = "Kullanıcı bilgisi alınamadı"
                }, 400, ct);
                return;
            }

            // Kullanıcının aktif sepetini al veya oluştur
            var cart = await _cartService.GetByUserId(userId.Value);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId.Value,
                    IsActive = true,
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now,
                    CartItems = new List<CartItems>()
                };
                _cartService.TAdd(cart);
            }
            // Cookie'den sepeti al
            var cartCookie = HttpContext.Request.Cookies["cart"];
            if (!string.IsNullOrEmpty(cartCookie))
            {
                var cookieItems = JsonConvert.DeserializeObject<List<CartItemViewModel>>(cartCookie);
                foreach (var cookieItem in cookieItems)
                {
                    var product = _productService.GetById(cookieItem.productId);
                    if (product == null) continue;
                    // Database'de bu ürün var mı kontrol et
                    var existingItem = cart.CartItems
                    .FirstOrDefault(ci => ci.ProductId == cookieItem.productId);
                    if (existingItem != null)
                    {
                        // Varsa miktarı güncelle
                        existingItem.Quantity += cookieItem.quantity;
                        existingItem.TotalPrice = existingItem.UnitPrice*existingItem.Quantity;
                        _cartItemsService.TUpdate(existingItem);
                    }
                    else
                    {
                        // Yoksa yeni ekle
                        var cartItem = new CartItems
                        {
                            CartId = cart.Id,
                            ProductId = cookieItem.productId,
                            Quantity = cookieItem.quantity,
                            VendorId = product.VendorId,
                            UnitPrice = product.UnitPrice,
                            TotalPrice = product.UnitPrice*cookieItem.quantity,                     
                        };
                        _cartItemsService.TAdd(cartItem);
                    }
                }
                // Cookie'yi temizle
                HttpContext.Response.Cookies.Delete("cart");
            }
            await SendAsync(new
            {
                success = true,
                message = "Sepet başarıyla senkronize edildi",
                cartId = cart.Id
            }, 200, ct);
        }
        catch (Exception ex)
        {
            await SendAsync(new
            {
                success = false,
                message = "Sepet senkronizasyonu sırasında hata oluştu"
            }, 500, ct);
        }
    }
}