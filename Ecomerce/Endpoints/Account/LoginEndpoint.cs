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
            new Claim("token", result.Token)
        };

            try
            {
                var claimsIdentity = new ClaimsIdentity(claims, "login");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(claimsPrincipal);
            }
            catch (Exception ex)
            {
                var errorResponse = new { success = false, message = ex.Message };
                await HttpContext.Response.WriteAsJsonAsync(errorResponse);
                return; 
            }
            await SendOkAsync(new {success=true, message = "Giriş başarılı", token = result.Token }, ct);
        }
        else
        {
            var errorResponse = new { success = false, message = result.Error };
            HttpContext.Response.StatusCode = 400;
            await HttpContext.Response.WriteAsJsonAsync(errorResponse); 
        }
    }
}