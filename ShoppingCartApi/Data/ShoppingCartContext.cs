using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.Models;

namespace ShoppingCartApi.Data
{
    public class ShoppingCartContext : DbContext
    {
        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
    
        // Seed default products
        public static void SeedData(ShoppingCartContext context)
        {
            // Check if there are any products in the database
            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product { Name = "Dell XPS", Price = 2500 },
                    new Product { Name = "Lenovo Yoga", Price = 1500 },
                    new Product { Name = "Thinkpad X2", Price = 2800 },
                    new Product { Name = "Asus Carbon", Price = 1900 },
                    new Product { Name = "Acer Aspire", Price = 1200 }
                );
                context.SaveChanges(); // Save the changes to the database
            }
        }
    
    }
}
