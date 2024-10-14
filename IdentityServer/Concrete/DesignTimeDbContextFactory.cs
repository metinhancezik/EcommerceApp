using IdentityServerLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace IdentityServerLayer.Concrete
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdentityServerDbContext>
    {
        public IdentityServerDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<IdentityServerDbContext>();
            var connectionString = configuration.GetConnectionString("IdentityServerConnection");

            builder.UseSqlServer(connectionString);

            return new IdentityServerDbContext(builder.Options);
        }
    }
}