using EshopApiAlza.Controllers;
using EshopApiAlza.Data;
using EshopApiAlza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EshopApiAlza.Tests.ControllerTests
{
    public class ProductsV2ControllerTests
    {
        private IConfiguration Configuration { get; set; }
        private bool UseMockData { get; set; }

        private async Task<AppDbContext> GetDatabaseContext()
        {
            Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .AddEnvironmentVariables()
                .Build();

            UseMockData = Configuration.GetValue<bool>("UseMockData");

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
        public async Task GetProducts_WithPagination_ReturnsCorrectPage()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var controller = new ProductsV2Controller(dbContext);

            // Act
            var result = await controller.GetProducts(page: 2, size: 10);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PaginatedResponse<Product>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var response = Assert.IsType<PaginatedResponse<Product>>(okResult.Value);

            Assert.Equal(20, response.TotalProducts);  // Total number of products
            Assert.Equal(2, response.TotalPages);     // 6 products, page size 2 = 3 pages
            Assert.Equal(2, response.CurrentPage);    // Requested page 1
            Assert.Equal(10, response.PageSize);       // Page size 2
            Assert.Equal(10, response.Data.Count());  // 2 products on page 1
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
