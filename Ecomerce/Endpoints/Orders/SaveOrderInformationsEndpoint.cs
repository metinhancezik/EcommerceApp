using ECommerceView.Models.Orders;
using EntityLayer.Concrete;
using FastEndpoints;
using ServiceLayer.Abstract;

namespace ECommerceView.Endpoints.Orders
{
    public class SaveOrderInformationsEndpoint : Endpoint<SaveOrderInformationsRequest>
    {
        private readonly IOrderInformationsService _orderInformationService; 
        private readonly IAuthTokensService _authTokensService;

        public SaveOrderInformationsEndpoint(
            IOrderInformationsService orderInformationService, 
            IAuthTokensService authTokensService)
        {
            _orderInformationService = orderInformationService; 
            _authTokensService = authTokensService;
        }

        public override void Configure()
        {
            Post("/api/orders/save-information");
            Claims("token");
        }

        public override async Task HandleAsync(SaveOrderInformationsRequest req, CancellationToken ct)
        {
            try
            {
                var token = User.Claims.FirstOrDefault(c => c.Type == "token")?.Value;
                if (string.IsNullOrEmpty(token))
                {
                    var errorResponse = new { success = false, message = "Oturum bilgisi bulunamadı" };
                    HttpContext.Response.StatusCode = 401;
                    await HttpContext.Response.WriteAsJsonAsync(errorResponse);
                    return;
                }

                var userId = await _authTokensService.GetUserIdFromTokenAsync(token);
                if (!userId.HasValue)
                {
                    var errorResponse = new { success = false, message = "Geçersiz oturum" };
                    HttpContext.Response.StatusCode = 401;
                    await HttpContext.Response.WriteAsJsonAsync(errorResponse);
                    return;
                }

                var orderInfo = new OrderInformations
                {
                    UserId = userId.Value,
                    Name = req.Name,
                    Surname = req.Surname,
                    Phone = req.Phone,
                    IdentityNumber = req.IdentityNumber,
                    Address = req.Address,
                    CityId = req.CityId,
                    DistrictId = req.DistrictId,
                    NeighborhoodId = req.NeighborhoodId,
                    CreatedTime = DateTime.Now
                };

                     _orderInformationService.TAdd(orderInfo); 
           
                    await HttpContext.Response.WriteAsJsonAsync(new
                    {
                        success = true,
                        message = "Bilgiler başarıyla kaydedildi",
                        redirectUrl = "/Pages/Payment"
                    });
           
            }
            catch (Exception ex)
            {
                var errorResponse = new { success = false, message = ex.Message };
                HttpContext.Response.StatusCode = 500;
                await HttpContext.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}
