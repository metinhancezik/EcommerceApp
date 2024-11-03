using AuthenticationLayer.Interfaces;
using ECommerceView.Endpoints.Interfaces;
using ECommerceView.Models;
using ECommerceView.Models.Account;
using ECommerceView.Models.Cart;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


public class LoginEndpoint : Endpoint<LoginViewModel>
{
    private readonly IAuthService _authService;
    private readonly ISyncCartToDatabaseEndpoint _syncCartEndpoint;
    public LoginEndpoint(IAuthService authService, ISyncCartToDatabaseEndpoint syncCartEndpoint)
    {
        _authService = authService;
        _syncCartEndpoint = syncCartEndpoint;
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
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, req.Email),
                    new Claim("token", result.Token)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "login");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(claimsPrincipal);

                // Cookie'yi set et
                HttpContext.Response.Cookies.Append("auth_token", result.Token, new CookieOptions
                {
                    HttpOnly = false,
                    Secure = false,
                    SameSite = SameSiteMode.Strict
                });

                // Token'ı parametre olarak geçir
                await SyncCart(result.Token);

                await SendOkAsync(new
                {
                    success = true,
                    message = "Giriş başarılı",
                    token = result.Token
                }, ct);
            }
            catch (Exception ex)
            {
                var errorResponse = new { success = false, message = ex.Message };
                await HttpContext.Response.WriteAsJsonAsync(errorResponse);
                return;
            }
        }
        else
        {
            var errorResponse = new { success = false, message = result.Error };
            HttpContext.Response.StatusCode = 400;
            await HttpContext.Response.WriteAsJsonAsync(errorResponse);
        }
    }

    private async Task SyncCart(string token)
    {
        try
        {
            var syncRequest = new SyncCartRequestModel();

            await _syncCartEndpoint.HandleAsync(syncRequest, new CancellationToken());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Sepet senkronizasyonu hatası: {ex.Message}");
        }
    }
}