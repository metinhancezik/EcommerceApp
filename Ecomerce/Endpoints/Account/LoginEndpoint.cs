using AuthenticationLayer.Interfaces;
using ECommerceView.Models;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


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

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, req.Email),
            };

              var claimsIdentity = new ClaimsIdentity(claims, "login");
              var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(claimsPrincipal); // Bu kısımda hata veriyor henüz

            await SendOkAsync(new { message = "Giriş başarılı", token = result.Token }, ct);
            await SendRedirectAsync("/Pages/Home");
        }
        else
        {
            await SendRedirectAsync("/Pages/Login?error=true&message=" + result.Error);
        }
    }
}