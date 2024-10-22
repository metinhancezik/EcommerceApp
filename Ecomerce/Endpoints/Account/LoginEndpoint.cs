using AuthenticationLayer.Interfaces;
using ECommerceView.Models;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;


public class LoginEndpoint : Endpoint<LoginViewModel>
{
    private readonly IAuthService _authService;

    public LoginEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Post("/api/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginViewModel req, CancellationToken ct)
    {
        var result = await _authService.ValidateCredentialsAsync(req.Email, req.Password);
        if (result.Success)
        {
            await SendOkAsync(new { message = "Giriş başarılı", token = result.Token }, ct);
        }
        else
        {
            await SendRedirectAsync("/Account/Login?error=true");
        }
    }
}