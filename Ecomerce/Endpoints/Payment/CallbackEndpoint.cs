using FastEndpoints;

namespace ECommerceView.Endpoints.Payment;

public class CallbackEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("/payment/callback");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
       
        var form = await HttpContext.Request.ReadFormAsync(ct);

        await SendOkAsync(new { message = "Callback işlendi" }, ct);
    }
}