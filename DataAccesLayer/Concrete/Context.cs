using Microsoft.EntityFrameworkCore;
using EntityLayer.Concrete; // UserDetails sınıfının bulunduğu namespace'i ekleyin

namespace DataAccesLayer.Concrete
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
        //For the Migration "Add-Migration AddUserDetailsTable -Context DataAccesLayer.Concrete.Context -Project DataAccesLayer -StartupProject ECommerceView"
        //For the Update database "Update-Database -Context DataAccesLayer.Concrete.Context -Project DataAccesLayer -StartupProject ECommerceView"
        public DbSet<UserDetails> UserDetails { get; set; }
        // Diğer DbSet'lerinizi burada tanımlayın
        // Örnek: public DbSet<YourEntity> YourEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // UserDetails için konfigürasyon
            modelBuilder.Entity<UserDetails>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.IdentityServerId).IsRequired();
                entity.HasIndex(e => e.IdentityServerId).IsUnique();
                // Diğer property konfigürasyonları...
            });

            // Diğer entity konfigürasyonlarınızı burada yapın
        }
    }
}