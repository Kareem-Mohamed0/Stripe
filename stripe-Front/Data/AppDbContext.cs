using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe.Models;

namespace Stripe.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<StoreProduct> StoreProducts { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed Products
            builder.Entity<StoreProduct>().HasData(
                new StoreProduct { Id = 1, Name = "Laptop", Price = 15000, ImageUrl = "/images/laptop.png" },
                new StoreProduct { Id = 2, Name = "Headphones", Price = 1200, ImageUrl = "/images/headphones.png" },
                new StoreProduct { Id = 3, Name = "Smartphone", Price = 9000, ImageUrl = "/images/phone.png" }
            );

            // Seed Roles
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Customer", NormalizedName = "CUSTOMER" }
            );
        }
    }
}
