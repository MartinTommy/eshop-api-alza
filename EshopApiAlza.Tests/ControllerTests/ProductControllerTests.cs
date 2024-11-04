using EshopApiAlza.Application.Commands;
using EshopApiAlza.Application.Queries;
using EshopApiAlza.Application.Responses;
using EshopApiAlza.Domain.Models;
using EshopApiAlza.Infrastructure.Data;
using EshopApiAlza.Presentation.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MediatR;
using Moq;

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

        private IMediator BuildRealMediator(AppDbContext dbContext)
        {
            var services = new ServiceCollection();

            // Register DbContext and MediatR handlers
            services.AddSingleton(dbContext);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetProductByIdQueryHandler).Assembly));
          
            // Build service provider and retrieve Mediator
            var provider = services.BuildServiceProvider();
            return provider.GetRequiredService<IMediator>();
        }

        private async Task<AppDbContext> GetDatabaseContext()
        {
            AppDbContext databaseContext;

            if (UseMockData)
            {
                // Use in-memory database for testing
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;

                databaseContext = new AppDbContext(options);
                await databaseContext.Database.EnsureCreatedAsync();

                // Seed mock data
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
                // Use actual database connection for integration tests
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
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
            IMediator mediator;

            if (UseMockData)
            {
                var mockMediator = new Mock<IMediator>();
                mockMediator
                    .Setup(m => m.Send(It.Is<UpdateProductDescriptionCommand>(cmd => cmd.ProductId == 1 && cmd.NewDescription == "New Description"), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(true)  // Simulate successful command handling
                    .Callback((IRequest<bool> command, CancellationToken ct) =>
                    {
                        var updateCommand = command as UpdateProductDescriptionCommand;

                        if (updateCommand != null)
                        {
                            var product = dbContext.Products.Find(updateCommand.ProductId);
                            if (product != null)
                            {
                                product.Description = updateCommand.NewDescription;
                                dbContext.SaveChanges();
                            }
                        }
                    });

                mediator = mockMediator.Object;
            }
            else
            {
                mediator = BuildRealMediator(dbContext);
            }

            var controller = new ProductsController(mediator);

            // Act
            var result = await controller.UpdateProductDescription(1, "New Description");

            // Assert
            Assert.IsType<NoContentResult>(result);

            var updatedProduct = await dbContext.Products.FindAsync(1);
            Assert.NotNull(updatedProduct);
            Assert.Equal("New Description", updatedProduct!.Description);
        }

        [Fact]
        public async Task UpdateProductDescription_ProductNotFound_ReturnsNotFound()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();

            IMediator mediator;
            if (UseMockData)
            {
                var mockMediator = new Mock<IMediator>();
                mockMediator
                    .Setup(m => m.Send(It.Is<UpdateProductDescriptionCommand>(cmd => cmd.ProductId == 999), default))
                    .ReturnsAsync(false);  // Simulate product not found
                mediator = mockMediator.Object;
            }
            else
            {
                mediator = BuildRealMediator(dbContext);
            }

            var controller = new ProductsController(mediator);

            // Act
            var result = await controller.UpdateProductDescription(999, "Non-existent Product Description");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetProductById_ProductExists_ReturnsOkWithProduct()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();

            IMediator mediator;
            if (UseMockData)
            {
                var mockMediator = new Mock<IMediator>();
                mockMediator
                    .Setup(m => m.Send(It.Is<GetProductByIdQuery>(q => q.ProductId == 1), default))
                    .ReturnsAsync(new Product() { Id = 1, Name = "Product 1", ImgUri = "https://example.com/img1.jpg", Price = 29.99m, Description = "First product description" });

                mediator = mockMediator.Object;
            }
            else
            {
                mediator = BuildRealMediator(dbContext);
            }

            var controller = new ProductsController(mediator);
            // Act
            var result = await controller.GetProductById(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Product>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var product = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(1, product.Id);
            Assert.Equal("Product 1", product.Name);
        }

        [Fact]
        public async Task GetProductById_ProductDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();

            IMediator mediator;
            if (UseMockData)
            {
                var mockMediator = new Mock<IMediator>();
                mockMediator
                    .Setup(m => m.Send(It.Is<GetProductByIdQuery>(q => q.ProductId == 999), default));
                mediator = mockMediator.Object;
            }
            else
            {
                mediator = BuildRealMediator(dbContext);
            }

            var controller = new ProductsController(mediator);


            // Act
            var result = await controller.GetProductById(999);  // Non-existent product ID

            // Assert
            var actionResult = Assert.IsType<ActionResult<Product>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 5)]
        [InlineData(1, 100)]
        public async Task GetProducts_v2_WithPagination_ReturnsCorrectPage(int page, int size)
        {
            // Arrange
            var dbContext = await GetDatabaseContext();

            IMediator mediator;
            if (UseMockData)
            {
                var mockMediator = new Mock<IMediator>();
                var totalProducts = await dbContext.Products.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalProducts / size);
                var products = await dbContext.Products
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToListAsync();

                var paginatedResponse = new PaginatedResponse<Product>(
                    totalProducts: totalProducts,
                    totalPages: totalPages,
                    currentPage: page,
                    pageSize: size,
                    data: products
                );

                mockMediator
                    .Setup(m => m.Send(It.Is<GetPaginatedProductsQuery>(q => q.Page == page && q.Size == size), default))
                    .ReturnsAsync(paginatedResponse);

                mediator = mockMediator.Object;
            }
            else
            {
                mediator = BuildRealMediator(dbContext);
            }

            var controller = new ProductsV2Controller(mediator);

            // Act
            var result = await controller.GetProducts(page: page, size: size);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PaginatedResponse<Product>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var response = Assert.IsType<PaginatedResponse<Product>>(okResult.Value);

            Assert.Equal(page, response.CurrentPage);
            Assert.Equal(size, response.PageSize);
        }


        [Fact]
        public async Task GetProducts_v2_PageExceedsTotal_ReturnsNotFound()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();

            IMediator mediator;
            if (UseMockData)
            {
                var mockMediator = new Mock<IMediator>();
                var totalProducts = await dbContext.Products.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalProducts / 1000);
                mockMediator
                    .Setup(m => m.Send(It.Is<GetPaginatedProductsQuery>(q => q.Page > totalPages), default))
                    .ReturnsAsync(new PaginatedResponse<Product>(
                                    totalProducts: 0,         
                                    totalPages: 0,            
                                    currentPage: 100,          
                                    pageSize: 1000,            
                                    data: new List<Product>()  
                    ));

                mediator = mockMediator.Object;
            }
            else
            {
                mediator = BuildRealMediator(dbContext);
            }

            var controller = new ProductsV2Controller(mediator);

            // Act
            var result = await controller.GetProducts(page: 100, size: 1000);  // Exceeds total pages

            // Assert
            var actionResult = Assert.IsType<ActionResult<PaginatedResponse<Product>>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }
    }
}
