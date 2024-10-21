using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace ECommerceView.Endpoints.Account;

public class LogoutEndpoint : EndpointWithoutRequest
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public LogoutEndpoint(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public override void Configure()
    {
        Post("/account/logout");
        Roles("User"); 
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await _signInManager.SignOutAsync();
        await SendOkAsync(new { message = "Çıkış yapıldı" }, ct);
    }
}