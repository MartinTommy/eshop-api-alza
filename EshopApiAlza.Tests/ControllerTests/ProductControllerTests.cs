using EshopApiAlza.Controllers;
using EshopApiAlza.Data;
using EshopApiAlza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EshopApiAlza.Tests.ControllerTests
{
    public class ProductControllerTests
    {
        private async Task<AppDbContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new AppDbContext(options);
            databaseContext.Database.EnsureCreated();

            if (!await databaseContext.Products.AnyAsync())
            {
                databaseContext.Products.AddRange(
                   new Product { Id = 1, Name = "Product 1", ImgUri = "https://example.com/img1.jpg", Price = 29.99m, Description = "First product" },
                   new Product { Id = 2, Name = "Product 2", ImgUri = "https://example.com/img2.jpg", Price = 49.99m, Description = "Second product" },
                   new Product { Id = 3, Name = "Product 3", ImgUri = "https://example.com/img3.jpg", Price = 19.99m, Description = "Third product" },
                   new Product { Id = 4, Name = "Product 4", ImgUri = "https://example.com/img4.jpg", Price = 39.99m, Description = "Fourth product" }
               );
                await databaseContext.SaveChangesAsync();
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