using ECommerceView.Models.Cart;

namespace ECommerceView.Endpoints.Interfaces
{
    public interface ISyncCartToDatabaseEndpoint
    {
        Task HandleAsync(SyncCartRequestModel req, CancellationToken ct);
    }
}
