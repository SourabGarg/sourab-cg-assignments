using Microsoft.EntityFrameworkCore;

namespace Product.Models
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            // Seed dummy data
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Price = 1200.00m, Category = "Electronics" },
                new Product { Id = 2, Name = "Mouse", Price = 25.50m, Category = "Accessories" },
                new Product { Id = 3, Name = "Keyboard", Price = 75.00m, Category = "Accessories" },
                new Product { Id = 4, Name = "Monitor", Price = 350.00m, Category = "Electronics" },
                new Product { Id = 5, Name = "Headphones", Price = 120.00m, Category = "Audio" }
            );
        }
    }
}
