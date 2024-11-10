using ECommerceView.Endpoints.Baskets;

namespace ECommerceView.Endpoints.Interfaces
{
    public interface IRemoveFromCartEndpoint
    {
        Task HandleAsync(RemoveFromCartRequestViewModel req, CancellationToken ct);
    }
}
