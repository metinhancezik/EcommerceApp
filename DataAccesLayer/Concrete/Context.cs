using Microsoft.EntityFrameworkCore;
using EntityLayer.Concrete;

namespace DataAccesLayer.Concrete
{
    public class Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=ECommerceDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<UserAuth> UserAuths { get; set; }
        public DbSet<AuthTokens> AuthTokens { get; set; }
        public DbSet<PasswordResets> PasswordResets { get; set; }
        public DbSet<Vendors> Vendors { get; set; }
        public DbSet<OrderInformations> OrderInformations { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<OrderState> OrderStates { get; set; }
        public DbSet<OrderHistory> OrderHistories { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Neighborhood> Neighborhoods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserDetails>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(u => u.Country)
                    .WithMany(c => c.UserDetails)
                    .HasForeignKey(u => u.CountryId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasQueryFilter(u => u.IsActive);
            });

            modelBuilder.Entity<UserAuth>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(ua => ua.User)
                    .WithOne()
                    .HasForeignKey<UserAuth>(ua => ua.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasQueryFilter(ua => ua.User.IsActive);
            });

            modelBuilder.Entity<AuthTokens>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(at => at.User)
                    .WithMany()
                    .HasForeignKey(at => at.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasQueryFilter(at => at.User.IsActive);
            });

            modelBuilder.Entity<PasswordResets>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(pr => pr.User)
                    .WithMany()
                    .HasForeignKey(pr => pr.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasQueryFilter(pr => pr.User.IsActive);
            });

            modelBuilder.Entity<Vendors>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<OrderInformations>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(o => o.User)
                    .WithMany()
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(o => o.Country)
                    .WithMany()
                    .HasForeignKey(o => o.CountryId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(o => o.City)
                    .WithMany()
                    .HasForeignKey(o => o.CityId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(o => o.District)
                    .WithMany()
                    .HasForeignKey(o => o.DistrictId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(o => o.Neighborhood)
                    .WithMany()
                    .HasForeignKey(o => o.NeighborhoodId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasQueryFilter(o => o.User.IsActive);
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(os => os.OrderInformation)
                    .WithMany(oi => oi.OrderStatuses)
                    .HasForeignKey(os => os.OrderInformationId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(os => os.State)
                    .WithMany(s => s.OrderStatuses)
                    .HasForeignKey(os => os.StateId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(os => os.Vendor)
                    .WithMany(v => v.OrderStatuses)
                    .HasForeignKey(os => os.VendorId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasQueryFilter(os => os.OrderInformation.User.IsActive);
            });

            modelBuilder.Entity<OrderState>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<OrderHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(oh => oh.OrderInformation)
                    .WithMany(oi => oi.OrderHistories)
                    .HasForeignKey(oh => oh.OrderInformationId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(oh => oh.State)
                    .WithMany(s => s.OrderHistories)
                    .HasForeignKey(oh => oh.StateId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasQueryFilter(oh => oh.OrderInformation.User.IsActive);
            });

            modelBuilder.Entity<OrderItems>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(oi => oi.TotalPrice).HasColumnType("decimal(18,2)");
                entity.Property(oi => oi.UnitPrice).HasColumnType("decimal(18,2)");
                entity.HasOne(oi => oi.OrderInformation)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(oi => oi.OrderInformationId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(oi => oi.Product)
                    .WithMany()
                    .HasForeignKey(oi => oi.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(oi => oi.Vendor)
                    .WithMany()
                    .HasForeignKey(oi => oi.VendorId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasQueryFilter(oi => oi.OrderInformation.User.IsActive);
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(p => p.UnitPrice).HasColumnType("decimal(18,2)");
                entity.HasOne(p => p.Vendor)
                    .WithMany(v => v.Products)
                    .HasForeignKey(p => p.VendorId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasQueryFilter(p => p.Vendor.IsActive);

            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(c => c.User)
                    .WithMany()
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasQueryFilter(c => c.User.IsActive);
            });

            modelBuilder.Entity<CartItems>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(ci => ci.TotalPrice).HasColumnType("decimal(18,2)");
                entity.Property(ci => ci.UnitPrice).HasColumnType("decimal(18,2)");
                entity.HasOne(ci => ci.Cart)
                    .WithMany(c => c.CartItems)
                    .HasForeignKey(ci => ci.CartId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(ci => ci.Product)
                    .WithMany()
                    .HasForeignKey(ci => ci.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(ci => ci.Vendor)
                    .WithMany()
                    .HasForeignKey(ci => ci.VendorId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasQueryFilter(ci => ci.Cart.User.IsActive);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(c => c.Country)
                    .WithMany(co => co.Cities)
                    .HasForeignKey(c => c.CountryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.City)
                    .WithMany(c => c.Districts)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Neighborhood>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(n => n.District)
                    .WithMany(d => d.Neighborhoods)
                    .HasForeignKey(n => n.DistrictId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}