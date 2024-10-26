using AuthenticationLayer.Interfaces;
using FastEndpoints;

public class LogoutEndpoint : Endpoint<LogoutRequest>
{
    private readonly IAuthService _authService;

    public LogoutEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Post("/api/logout");
    }

    public override async Task HandleAsync(LogoutRequest req, CancellationToken ct)
    {
        if (await _authService.RevokeTokenAsync(req.Token))
        {
            await SendOkAsync(new { message = "Çıkış yapıldı" }, ct);
        }
        else
        {
            await SendUnauthorizedAsync(ct);
        }
    }
}

public class LogoutRequest
{
    public string Token { get; set; }
}