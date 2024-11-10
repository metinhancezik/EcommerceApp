using ECommerceView.Endpoints.Baskets;
using ECommerceView.Endpoints.Interfaces;
using ECommerceView.Models.Orders;
using EntityLayer.Concrete;
using FastEndpoints;
using Iyzipay.Model;
using ServiceLayer.Abstract;
using System.Transactions;

namespace ECommerceView.Endpoints.Orders
{
    public class CompleteOrderEndpoint : Endpoint<CompleteOrderRequest, CompleteOrderResponse>, ICompleteOrderEndpoint
    {
        private readonly IOrderInformationsService _orderInformationsService;
        private readonly IOrderStatusService _orderStatusService;
        private readonly IOrderItemsService _orderItemsService;
        private readonly IOrderHistoryService _orderHistoryService;
        private readonly ICartService _cartService;
        private readonly ICartItemsService _cartItemsService;
        private readonly IRemoveFromCartEndpoint _removeFromCartEndpoint;
        private readonly ILogger<CompleteOrderEndpoint> _logger;

        public CompleteOrderEndpoint(
            IOrderInformationsService orderInformationsService,
            IOrderStatusService orderStatusService,
            IOrderItemsService orderItemsService,
        IOrderHistoryService orderHistoryService,
            ICartService cartService,
            ICartItemsService cartItemsService,
            IRemoveFromCartEndpoint removeFromCartEndpoint,
            ILogger<CompleteOrderEndpoint> logger)
        {
            _orderInformationsService = orderInformationsService;
            _orderStatusService = orderStatusService;
            _orderItemsService = orderItemsService;
            _orderHistoryService = orderHistoryService;
            _cartService = cartService;
            _cartItemsService = cartItemsService;
            _removeFromCartEndpoint = removeFromCartEndpoint;
            _logger = logger;
        }

        public override void Configure()
        {
            Post("/api/orders/complete");
        }

        public override async Task HandleAsync(CompleteOrderRequest req, CancellationToken ct)
        {
            if (req == null || req.Items == null || !req.Items.Any())
            {
                await SendAsync(new CompleteOrderResponse
                {
                    Success = false,
                    Message = "Geçersiz sipariş bilgileri"
                }, 400, ct);
                return;
            }

            try
            {
                // Önceden kaydedilmiş OrderInformation'ı al
                var orderInfo = await _orderInformationsService.GetLastOrderByUserId(req.UserId);
                if (orderInfo == null)
                {
                    _logger.LogError($"Kullanıcı için sipariş bilgisi bulunamadı. UserId: {req.UserId}");
                    await SendAsync(new CompleteOrderResponse
                    {
                        Success = false,
                        Message = "Sipariş bilgileri bulunamadı"
                    }, 400, ct);
                    return;
                }

                // Transaction başlat
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                try
                {
                    // 1. OrderStatus oluştur
                    var orderStatus = new OrderStatus
                    {
                        OrderInformationId = orderInfo.Id,
                        StateId = 1, // Sipariş alındı durumu
                        PaymentStatus = true,
                        UpdatedTime = DateTime.Now
                    };
                    _orderStatusService.TAdd(orderStatus);

                    // 2. OrderItems oluştur ve sepetten kaldır
                    foreach (var item in req.Items)
                    {
                        if (item.ProductId <= 0 || item.Quantity <= 0)
                        {
                            continue; // Geçersiz ürünleri atla
                        }

                        var orderItem = new OrderItems
                        {
                            OrderInformationId = orderInfo.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            TotalPrice = item.TotalPrice,
                            VendorId = item.VendorId
                        };
                        _orderItemsService.TAdd(orderItem);

                        
                        // Sepetten toplu silme işlemi
                        var cart = await _cartService.GetByUserId(req.UserId);
                        if (cart != null)
                        {
                            // Silinecek ürün ID'lerini bir listeye dönüştür
                            var productIds = new List<long> { item.ProductId };

                            var cartItems = await _cartItemsService.GetByCartAndProductIds(cart.Id, productIds);
                            if (cartItems.Any())
                            {
                                await _cartItemsService.DeleteCartItems(cartItems);
                            }
                        }
                    }

                    // 3. OrderHistory oluştur
                    var orderHistory = new OrderHistory
                    {
                        OrderInformationId = orderInfo.Id,
                        StateId = 1, // İlk durum
                        StatusUpdatedTime = DateTime.Now
                    };
                    _orderHistoryService.TAdd(orderHistory);

                    // Transaction'ı tamamla
                    transaction.Complete();

                    _logger.LogInformation($"Sipariş başarıyla tamamlandı. OrderId: {orderInfo.Id}, UserId: {req.UserId}"); 
                }
                catch (Exception ex)
                {
                    // Transaction otomatik olarak rollback olacak
                    _logger.LogError(ex, $"Sipariş işlemi sırasında hata. UserId: {req.UserId}, OrderId: {orderInfo.Id}");
                    throw; // Üst catch bloğunda yakalanacak
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Sipariş tamamlanırken hata oluştu. UserId: {req.UserId}");
                await SendAsync(new CompleteOrderResponse
                {
                    Success = false,
                    Message = "Siparişiniz alınırken bir hata oluştu. Lütfen daha sonra tekrar deneyiniz."
                }, 500, ct);
            }
        }
    }
}
