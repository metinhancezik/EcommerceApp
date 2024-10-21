using FastEndpoints;
using ECommerceView.Models;

namespace ECommerceView.Endpoints.Account;

public class LoginEndpoint : Endpoint<LoginViewModel>
{
    public override void Configure()
    {
        Post("/account/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginViewModel req, CancellationToken ct)
    {
        // Burada login işlemlerini gerçekleştirin
        // Örnek olarak:
        if (req.Email == "test@example.com" && req.Password == "password")
        {
            await SendOkAsync(new { message = "Giriş başarılı", userId = "1" }, ct);
        }
        else
        {
            await SendUnauthorizedAsync(ct);
        }
    }
}