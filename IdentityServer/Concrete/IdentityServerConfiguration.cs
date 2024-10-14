using IdentityServerLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Duende.IdentityServer.EntityFramework.Options;
using System.IO;

namespace IdentityServerLayer.Concrete
{
    public static class IdentityServerConfiguration
    {
        public static IServiceCollection AddIdentityServerServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Yeni konfigürasyon oluştur
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddConfiguration(configuration);
            var config = builder.Build();
            var connectionString = config.GetConnectionString("IdentityServerConnection");
       
            services.AddDbContext<IdentityServerDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("IdentityServerConnection")));

            services.Configure<ConfigurationStoreOptions>(options => { });
            services.Configure<OperationalStoreOptions>(options => { });

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityServerDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
            })
             .AddAspNetIdentity<IdentityUser>()
             .AddConfigurationStore<IdentityServerDbContext>(options =>
             {
                 options.ConfigureDbContext = b => b.UseSqlServer(config.GetConnectionString("IdentityServerConnection"));
             })
             .AddOperationalStore<IdentityServerDbContext>(options =>
             {
                 options.ConfigureDbContext = b => b.UseSqlServer(config.GetConnectionString("IdentityServerConnection"));
                 options.EnableTokenCleanup = true;
                 options.TokenCleanupInterval = 3600; // Her saat başı
             })
             .AddDeveloperSigningCredential();

            return services;
        }
    }
}