//using FastEndpoints;
//using ECommerceView.Models; // CartItemViewModel sınıfının bulunduğu namespace
//using Microsoft.AspNetCore.Http;
//using Newtonsoft.Json;

//namespace ECommerceView.Endpoints.Baskets
//{
//    public class SyncCartToDatabaseEndpoint : Endpoint<SyncCartRequestViewModel>
//    {
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly ICartRepository _cartRepository; // Veritabanı işlemleri için bir repository

//        public SyncCartToDatabaseEndpoint(IHttpContextAccessor httpContextAccessor, ICartRepository cartRepository)
//        {
//            _httpContextAccessor = httpContextAccessor;
//            _cartRepository = cartRepository;
//        }

//        public override void Configure()
//        {
//            Post("api/cart/sync");
//        }

//        public override async Task HandleAsync(SyncCartRequestViewModel req, CancellationToken ct)
//        {
//            // Cookie'den mevcut sepeti al
//            var cart = GetCartFromCookie();

//            foreach (var item in cart)
//            {
//                // Veritabanında mevcut ürünü kontrol et
//                var existingItem = await _cartRepository.GetCartItemAsync(item.productId);
//                if (existingItem != null)
//                {
//                    // Eğer varsa, miktarı güncelle
//                    existingItem.quantity += item.quantity;
//                    await _cartRepository.UpdateCartItemAsync(existingItem);
//                }
//                else
//                {
//                    // Eğer yoksa, yeni bir kayıt oluştur
//                    await _cartRepository.AddCartItemAsync(item);
//                }
//            }

//            await SendOkAsync();
//        }

//        private List<CartItemViewModel> GetCartFromCookie()
//        {
//            var cart = _httpContextAccessor.HttpContext.Request.Cookies["cart"];
//            return string.IsNullOrEmpty(cart) ? new List<CartItemViewModel>() : JsonConvert.DeserializeObject<List<CartItemViewModel>>(cart);
//        }
//    }

//    public class SyncCartRequestViewModel
//    {
//        // Gerekirse ek bilgiler ekleyebilirsiniz
//    }
//}