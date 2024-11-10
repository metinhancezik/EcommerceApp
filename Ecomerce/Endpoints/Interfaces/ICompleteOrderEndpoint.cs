using ECommerceView.Models.Orders;

namespace ECommerceView.Endpoints.Interfaces
{
    public interface ICompleteOrderEndpoint
    {
        Task HandleAsync(CompleteOrderRequest req, CancellationToken ct);
    }
}
