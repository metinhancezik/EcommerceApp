using Microsoft.AspNetCore.Http;
using AuthenticationLayer.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationLayer.Middleware
{
    public class TokenAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authService = context.RequestServices.GetRequiredService<IAuthService>();

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null && await authService.ValidateTokenAsync(token))
            {
                // Token geçerli
                // Burada gerekli işlemleri yapabilirsiniz, örneğin:
                // context.Items["UserId"] = ... // Token'dan çıkarılan kullanıcı ID'si
            }

            await _next(context);
        }
    }

    public static class TokenAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenAuthenticationMiddleware>();
        }
    }
}