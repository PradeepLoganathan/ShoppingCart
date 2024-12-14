using Microsoft.EntityFrameworkCore;
using ProductApi.Models;


namespace ProductApi.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }
            
        // Seed default products
        public static void SeedData(ProductContext context)
        {
            // Check if there are any products in the database
            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product { Name = "Dell XPS", Price = 2500 },
                    new Product { Name = "Lenovo Yoga", Price = 1500 },
                    new Product { Name = "Thinkpad X2", Price = 2800 },
                    new Product { Name = "Asus Carbon", Price = 1900 },
                    new Product { Name = "Acer Aspire", Price = 1200 },
                    new Product { Name = "HP Pavilion", Price = 1400 },
                    new Product { Name = "HP Envy", Price = 1600 },
                    new Product { Name = "Surface Pro", Price = 2200 },
                    new Product { Name = "MacBook Pro", Price = 3200 },
                    new Product { Name = "MacBook Air", Price = 2000 },
                    new Product { Name = "Lenovo ThinkPad T14", Price = 2400 },
                    new Product { Name = "Dell Inspiron", Price = 1100 },
                    new Product { Name = "Asus ZenBook", Price = 1800 },
                    new Product { Name = "Razer Blade", Price = 3000 },
                    new Product { Name = "Alienware M15", Price = 3500 },
                    new Product { Name = "MSI Stealth", Price = 2700 },
                    new Product { Name = "Samsung Galaxy Book", Price = 1300 },
                    new Product { Name = "LG Gram", Price = 2100 },
                    new Product { Name = "HP Spectre x360", Price = 2300 },
                    new Product { Name = "Dell Latitude", Price = 1600 },
                    new Product { Name = "Acer Swift", Price = 1000 },
                    new Product { Name = "Asus ROG Zephyrus", Price = 2900 },
                    new Product { Name = "Lenovo Legion", Price = 2500 },
                    new Product { Name = "Surface Laptop", Price = 2150 },
                    new Product { Name = "Acer Nitro", Price = 1400 },
                    new Product { Name = "Dell G5", Price = 1700 },
                    new Product { Name = "Lenovo IdeaPad", Price = 900 },
                    new Product { Name = "HP Omen", Price = 2600 },
                    new Product { Name = "Acer Predator", Price = 3100 },
                    new Product { Name = "Asus TUF Gaming", Price = 1500 },
                    new Product { Name = "MSI Prestige", Price = 1750 },
                    new Product { Name = "Huawei MateBook", Price = 1250 },
                    new Product { Name = "Google Pixelbook", Price = 2300 }
                );
                context.SaveChanges(); // Save the changes to the database
            }
        }
    
    }
}
