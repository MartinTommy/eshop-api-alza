using Microsoft.EntityFrameworkCore;
using EshopApiAlza.Models;

namespace EshopApiAlza.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                    new Product { Id = 1, Name = "Product 1", ImgUri = "https://example.com/img1.jpg", Price = 29.99m, Description = "First product description" },
                    new Product { Id = 2, Name = "Product 2", ImgUri = "https://example.com/img2.jpg", Price = 49.99m, Description = "Second product description" },
                    new Product { Id = 3, Name = "Product 3", ImgUri = "https://example.com/img3.jpg", Price = 69.99m, Description = "Third product description" },
                    new Product { Id = 4, Name = "Product 4", ImgUri = "https://example.com/img4.jpg", Price = 89.99m, Description = "Fourth product description" },
                    new Product { Id = 5, Name = "Product 5", ImgUri = "https://example.com/img5.jpg", Price = 109.99m, Description = "Fifth product description" },
                    new Product { Id = 6, Name = "Product 6", ImgUri = "https://example.com/img6.jpg", Price = 129.99m, Description = "Sixth product description" }
            );
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
