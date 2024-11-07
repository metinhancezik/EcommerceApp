using ECommerceView.Models.Addres;
using FastEndpoints;
using ServiceLayer.Abstract;

namespace ECommerceView.Endpoints.Address
{
    public class GetDistrictsEndpoint : Endpoint<GetDistrictsRequest>
    {
        private readonly IDistrictService _districtService;

        public GetDistrictsEndpoint(IDistrictService districtService)
        {
            _districtService = districtService;
        }

        public override void Configure()
        {
            Get("/api/locations/districts/{CityId}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetDistrictsRequest req, CancellationToken ct)
        {
            try
            {
                var districts = _districtService.GetDistrictsByCityId(req.CityId)
                    .Select(d => new { value = d.Id, text = d.DistrictName })
                    .ToList();

                await SendOkAsync(districts, ct);
            }
            catch (Exception ex)
            {
                var errorResponse = new { success = false, message ="İlçeler getirilirken bir hata oldu! "};
                HttpContext.Response.StatusCode = 400;
                await HttpContext.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}
