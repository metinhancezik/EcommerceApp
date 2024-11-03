using AuthenticationLayer.Interfaces;
using ECommerceView.Models.Cart;
using EntityLayer.Concrete;
using FastEndpoints;
using Newtonsoft.Json;
using ServiceLayer.Abstract;

public class AddToCartEndpoint : Endpoint<CartItemViewModel>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICartService _cartService;
    private readonly IAuthService _authService;
    private readonly IAuthTokensService _authTokensService;
    private readonly ICartItemsService _cartItemsService;
    private readonly IProductsService _productService;

    public AddToCartEndpoint(
        IHttpContextAccessor httpContextAccessor,
        ICartService cartService,
        IAuthService authService,
        IAuthTokensService authTokensService,
        ICartItemsService cartItemsService,
        IProductsService productService)
    {
        _httpContextAccessor = httpContextAccessor;
        _cartService = cartService;
        _authService = authService;
        _authTokensService = authTokensService;
        _cartItemsService = cartItemsService;
        _productService = productService;
    }

    public override void Configure()
    {
        Post("api/cart/add");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CartItemViewModel req, CancellationToken ct)
    {
        try
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
                    productName = req.productName,
                    quantity = req.quantity,
                    price = req.price,
                });
            }
            SaveCartToCookie(cart);

            
            var token = HttpContext.Request.Cookies["auth_token"];
            if (!string.IsNullOrEmpty(token) && await _authService.ValidateTokenAsync(token))
            {
                var userId = await _authTokensService.GetUserIdFromTokenAsync(token);
                if (userId.HasValue)
                {
                    // Database'den mevcut sepeti al veya yeni oluştur
                    var dbCart = await _cartService.GetByUserId(userId.Value);
                    if (dbCart == null)
                    {
                        dbCart = new Cart
                        {
                            UserId = userId.Value,
                            IsActive = true,
                            CreatedTime = DateTime.Now,
                            UpdatedTime = DateTime.Now
                        };
                        _cartService.TAdd(dbCart);
                    }

                    // Ürün bilgilerini al
                    var product = _productService.GetById(req.productId);
                    if (product != null)
                    {
                        // Sepette bu üründen var mı kontrol et
                        var existingItem = dbCart.CartItems
                            .FirstOrDefault(ci => ci.ProductId == req.productId);

                        if (existingItem != null)
                        {
                            existingItem.Quantity += req.quantity;
                            existingItem.TotalPrice = existingItem.UnitPrice * existingItem.Quantity;
                            _cartItemsService.TUpdate(existingItem);
                        }
                        else
                        {
                            var cartItem = new CartItems
                            {
                                CartId = dbCart.Id,
                                ProductId = req.productId,
                                Quantity = req.quantity,
                                VendorId = product.VendorId,
                                UnitPrice = product.UnitPrice,
                                TotalPrice = product.UnitPrice * req.quantity
                            };
                            _cartItemsService.TAdd(cartItem);
                        }
                    }
                }
            }

            await SendAsync(new
            {
                success = true,
                message = "Ürün sepete eklendi",
                cartItemCount = cart.Count
            }, 200, ct);
        }
        catch (Exception ex)
        {
            await SendAsync(new
            {
                success = false,
                message = "Ürün sepete eklenirken bir hata oluştu"
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
}