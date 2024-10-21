using EshopApiAlza.Controllers;
using EshopApiAlza.Data;
using EshopApiAlza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EshopApiAlza.Tests.ControllerTests
{
    public class ProductsV2ControllerTests
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
        public async Task GetProducts_WithPagination_ReturnsCorrectPage()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var controller = new ProductsV2Controller(dbContext);

            // Act
            var result = await controller.GetProducts(page: 1, size: 2);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PaginatedResponse<Product>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var response = Assert.IsType<PaginatedResponse<Product>>(okResult.Value);

            Assert.Equal(6, response.TotalProducts);  // Total number of products
            Assert.Equal(3, response.TotalPages);     // 6 products, page size 2 = 3 pages
            Assert.Equal(1, response.CurrentPage);    // Requested page 1
            Assert.Equal(2, response.PageSize);       // Page size 2
            Assert.Equal(2, response.Data.Count());  // 2 products on page 1
        }

        [Fact]
        public async Task GetProducts_PageExceedsTotal_ReturnsNotFound()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var controller = new ProductsV2Controller(dbContext);

            // Act
            var result = await controller.GetProducts(page: 3, size: 20);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PaginatedResponse<Product>>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }
    }
}
