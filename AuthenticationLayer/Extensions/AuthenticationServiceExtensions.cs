using AuthenticationLayer.Interfaces;
using AuthenticationLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationLayer.Extensions
{
    public static class AuthenticationServiceExtensions
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}
