using EshopApiAlza.Controllers;
using EshopApiAlza.Data;
using EshopApiAlza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EshopApiAlza.Tests.ControllerTests
{
    public class ProductControllerTests
    {
        private IConfiguration Configuration { get; }
        private bool UseMockData { get; }

        public ProductControllerTests()
        {
            var baseDirectory = Directory.GetParent(AppContext.BaseDirectory)?.FullName
            ?? throw new InvalidOperationException("Unable to determine the parent directory.");

            Configuration = new ConfigurationBuilder()
                .SetBasePath(baseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            UseMockData = Configuration.GetValue<bool>("UseMockData");
        }

        private async Task<AppDbContext> GetDatabaseContext()
        {
            AppDbContext databaseContext;

            if (UseMockData)
            {
                var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

                databaseContext = new AppDbContext(options);
                await databaseContext.Database.EnsureCreatedAsync();

                var existingProducts = await databaseContext.Products.ToListAsync();
                databaseContext.Products.RemoveRange(existingProducts);

                databaseContext.Products.AddRange(
                    Enumerable.Range(1, 20).Select(i => new Product
                    {
                        Id = i,
                        Name = $"Product {i}",
                        ImgUri = $"https://example.com/img{i}.jpg",
                        Price = 20.0m + i,
                        Description = $"Product {i} description"
                    })
                        );
                await databaseContext.SaveChangesAsync();
            }
            else
            {
                var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(Configuration.GetValue<string>("ConnectionString"))
                .Options;

                databaseContext = new AppDbContext(options);
                await databaseContext.Database.EnsureCreatedAsync();
            }


            return databaseContext;
        }

        [Fact]
        public async Task UpdateProductDescription_ProductExists_ReturnsNoContent()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var controller = new ProductsController(dbContext);

            // Act
            var result = await controller.UpdateProductDescription(1, "New Description");

            // Assert
            Assert.IsType<NoContentResult>(result);

            var updatedProduct = await dbContext.Products.FindAsync(1);
            Assert.Equal("New Description", updatedProduct.Description);
        }

        [Fact]
        public async Task UpdateProductDescription_ProductNotFound_ReturnsNotFound()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var controller = new ProductsController(dbContext);

            // Act
            var result = await controller.UpdateProductDescription(999, "Non-existent Product Description");

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetProductById_ProductExists_ReturnsOkWithProduct()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var controller = new ProductsController(dbContext);

            // Act
            var result = await controller.GetProductById(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Product>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var product = Assert.IsType<Product>(okResult.Value);

            // Verify the returned product
            Assert.Equal(1, product.Id);
            Assert.Equal("Product 1", product.Name);
        }

        [Fact]
        public async Task GetProductById_ProductDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var controller = new ProductsController(dbContext);

            // Act
            var result = await controller.GetProductById(999);  // Non-existent product ID

            // Assert
            var actionResult = Assert.IsType<ActionResult<Product>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);

        }
    }
}