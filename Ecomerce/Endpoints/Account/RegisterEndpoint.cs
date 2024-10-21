using FastEndpoints;
using ECommerceView.Models;
using Microsoft.AspNetCore.Identity;

namespace ECommerceView.Endpoints.Account;

public class RegisterEndpoint : Endpoint<RegisterViewModel>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public RegisterEndpoint(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public override void Configure()
    {
        Post("/account/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterViewModel req, CancellationToken ct)
    {
        var user = new IdentityUser { UserName = req.Email, Email = req.Email };
        var result = await _userManager.CreateAsync(user, req.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            await SendOkAsync(new { message = "Kayıt başarılı", userId = user.Id }, ct);
        }
        else
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            await SendErrorsAsync(400, ct); 
        }
    }
}