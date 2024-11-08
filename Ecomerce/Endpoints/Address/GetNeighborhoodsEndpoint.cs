using ECommerceView.Models.Addres;
using FastEndpoints;
using ServiceLayer.Abstract;

namespace ECommerceView.Endpoints.Address
{
    public class GetNeighborhoodsEndpoint : Endpoint<GetNeighborhoodsRequest>
    {
        private readonly INeighborhoodService _neighborhoodService;

        public GetNeighborhoodsEndpoint(INeighborhoodService neighborhoodService)
        {
            _neighborhoodService = neighborhoodService;
        }

        public override void Configure()
        {
            Get("/api/locations/neighborhoods/{DistrictId}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetNeighborhoodsRequest req, CancellationToken ct)
        {
            try
            {
                var neighborhoods = _neighborhoodService.GetNeighborhoodsByDistrictId(req.DistrictId)
                    .Select(n => new { value = n.Id, text = n.NeighborhoodName })
                    .ToList();

                await SendOkAsync(neighborhoods, ct);
            }
            catch (Exception ex)
            {
                var errorResponse = new { success = false, message = "Mahalleler getirilirken bir hata oldu!" };
                HttpContext.Response.StatusCode = 400;
                await HttpContext.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}
