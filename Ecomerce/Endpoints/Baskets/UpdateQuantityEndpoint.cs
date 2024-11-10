using ECommerceView.Models.Cart;
using FastEndpoints;
using ServiceLayer.Abstract;

namespace ECommerceView.Endpoints.Baskets
{
    public class UpdateQuantityEndpoint : Endpoint<UpdateQuantityRequest, UpdateQuantityResponse>
    {
        private readonly ICartService _cartService;
        private readonly ICartItemsService _cartItemsService;
        private readonly IAuthTokensService _authTokensService;
        private readonly ILogger<UpdateQuantityEndpoint> _logger;

        public UpdateQuantityEndpoint(
            ICartService cartService,
            ICartItemsService cartItemsService,
            ILogger<UpdateQuantityEndpoint> logger,
            IAuthTokensService authTokensService)
        {
            _cartService = cartService;
            _cartItemsService = cartItemsService;
            _logger = logger;
            _authTokensService=authTokensService;
        }

        public override void Configure()
        {
            Post("/api/cart/updateQuantity");
            AllowAnonymous();
        }

        public override async Task HandleAsync(UpdateQuantityRequest req, CancellationToken ct)
        {
            try
            {
                
                var token = HttpContext.Request.Cookies["auth_token"];
                var userId = await _authTokensService.GetUserIdFromTokenAsync(token);

                var cart = await _cartService.GetByUserId(userId.Value);

                if (cart == null)
                {
                    await SendAsync(new UpdateQuantityResponse
                    {
                        Success = false,
                        Message = "Sepet bulunamadı"
                    }, 404, ct);
                    return;
                }

               
                var cartItem = await _cartItemsService.GetSingleCartItem(cart.Id, req.ProductId);
                if (cartItem == null)
                {
                    await SendAsync(new UpdateQuantityResponse
                    {
                        Success = false,
                        Message = "Ürün sepette bulunamadı"
                    }, 404, ct);
                    return;
                }

                var newQuantity = cartItem.Quantity + req.Change;

              
                if (newQuantity < 1)
                {
                    
                     _cartItemsService.TDelete(cartItem);

                    await SendAsync(new UpdateQuantityResponse
                    {
                        Success = true,
                        Message = "Ürün sepetten kaldırıldı",
                        NewQuantity = 0,
                        NewTotal = 0
                    }, cancellation: ct);
                    return;
                }

               
                const int maxQuantity = 10;
                if (newQuantity > maxQuantity)
                {
                    await SendAsync(new UpdateQuantityResponse
                    {
                        Success = false,
                        Message = $"En fazla {maxQuantity} adet ürün ekleyebilirsiniz",
                        NewQuantity = cartItem.Quantity,
                        NewTotal = cartItem.Quantity * cartItem.UnitPrice
                    }, 400, ct);
                    return;
                }

              
                cartItem.Quantity = newQuantity;
                 _cartItemsService.TUpdate(cartItem);

                var newTotal = newQuantity * cartItem.UnitPrice;

                _logger.LogInformation($"Sepet güncellendi. UserId: {userId}, ProductId: {req.ProductId}, NewQuantity: {newQuantity}");

                await SendAsync(new UpdateQuantityResponse
                {
                    Success = true,
                    Message = "Miktar güncellendi",
                    NewQuantity = newQuantity,
                    NewTotal = newTotal
                }, cancellation: ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Miktar güncellenirken hata oluştu");
                await SendAsync(new UpdateQuantityResponse
                {
                    Success = false,
                    Message = "Miktar güncellenirken bir hata oluştu"
                }, 500, ct);
            }
        }
    }
}
