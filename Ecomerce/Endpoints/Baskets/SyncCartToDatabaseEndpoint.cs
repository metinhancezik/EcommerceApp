using AuthenticationLayer.Interfaces;
using ECommerceView.Endpoints.Interfaces;
using ECommerceView.Models.Cart;
using EntityLayer.Concrete;
using FastEndpoints;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
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

            var token = req.Token;

            if (string.IsNullOrEmpty(token))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

           
            if (!await _authService.ValidateTokenAsync(token))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

         
            var userId = await _authTokensService.GetUserIdFromTokenAsync(token);
            if (!userId.HasValue)
            {
            


                return;
            }

           
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
           
            var cartCookie = _httpContextAccessor.HttpContext.Request.Cookies["cart"];
            if (!string.IsNullOrEmpty(cartCookie))
            {
                var cookieItems = JsonConvert.DeserializeObject<List<CartItemViewModel>>(cartCookie);
                foreach (var cookieItem in cookieItems)
                {
                    var product = _productService.GetProductByLongId(cookieItem.productId);  
                    if (product == null) continue;
                    
                    var existingItem = cart.CartItems
                    .FirstOrDefault(ci => ci.ProductId == cookieItem.productId);
                    if (existingItem != null)
                    {
                       
                        existingItem.Quantity += cookieItem.quantity;
                        existingItem.TotalPrice = existingItem.UnitPrice*existingItem.Quantity;
                        _cartItemsService.TUpdate(existingItem);
                    }
                    else
                    {
                      
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
                
                _httpContextAccessor.HttpContext.Response.Cookies.Delete("cart");
            }
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Detaylı hata: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");

            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                Console.WriteLine($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
            }

            await SendAsync(new
            {
                success = false,
                message = "Sepet senkronizasyonu sırasında hata oluştu",
                error = ex.Message,
                stackTrace = ex.StackTrace,
                innerError = ex.InnerException?.Message
            }, 500, ct);
        }
    }
}