using FastEndpoints;
using ECommerceView.Models;
using AuthenticationLayer.Interfaces;
using AuthenticationLayer.Models;

namespace ECommerceView.Endpoints.Account;

public class RegisterEndpoint : Endpoint<RegisterViewModel>
{
    private readonly IAuthService _authService;

    public RegisterEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Post("/api/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterViewModel req, CancellationToken ct)
    {
   
        var userRegistrationModel = new UserRegistrationModel
        {
            UserName = req.UserName,
            UserSurname = req.UserSurname,
            UserMail = req.UserMail,
            UserPhone = req.UserPhone,
            CountryId = req.CountryId,
            Password = req.Password
        };

        var result = await _authService.RegisterUserAsync(userRegistrationModel);

        if (result.Success)
        {
            var loginResult = await _authService.ValidateCredentialsAsync(req.UserMail, req.Password);
            if (loginResult.Success)
            {
                await SendOkAsync(new { message = "Kayıt ve giriş başarılı", token = loginResult.Token }, ct);
            }
            else
            {
                await SendOkAsync(new { message = "Kayıt başarılı, ancak giriş yapılamadı" }, ct);
            }
        }
        else
        {
            await SendAsync(new
            {
                statusCode = 400,
                message = result.Message
            }, 400, ct);
        }
    }
}