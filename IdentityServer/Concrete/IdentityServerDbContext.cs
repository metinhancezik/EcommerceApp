using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Extensions;
using Duende.IdentityServer.EntityFramework.Interfaces;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.Identity;

namespace IdentityServerLayer.Concrete
{
    public class IdentityServerDbContext : IdentityDbContext<IdentityUser>, IPersistedGrantDbContext, IConfigurationDbContext
    {
        public IdentityServerDbContext(DbContextOptions<IdentityServerDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var operationalStoreOptions = new OperationalStoreOptions();
            var configurationStoreOptions = new ConfigurationStoreOptions();

            builder.ConfigurePersistedGrantContext(operationalStoreOptions);
            builder.ConfigureResourcesContext(configurationStoreOptions);
            builder.ConfigureClientContext(configurationStoreOptions);
            builder.ConfigureIdentityProviderContext(configurationStoreOptions);
        }


        public DbSet<PersistedGrant> PersistedGrants { get; set; }
        public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }
        public DbSet<Key> Keys { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<IdentityResource> IdentityResources { get; set; }
        public DbSet<ApiResource> ApiResources { get; set; }
        public DbSet<ApiScope> ApiScopes { get; set; }
        public DbSet<ServerSideSession> ServerSideSessions { get; set; }
        public DbSet<PushedAuthorizationRequest> PushedAuthorizationRequests { get; set; }
        public DbSet<IdentityProvider> IdentityProviders { get; set; }
        public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }
    }
}