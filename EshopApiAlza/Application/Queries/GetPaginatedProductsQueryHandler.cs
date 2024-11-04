using EshopApiAlza.Application.Responses;
using EshopApiAlza.Domain.Models;
using EshopApiAlza.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EshopApiAlza.Application.Queries
{
    public class GetPaginatedProductsQueryHandler : IRequestHandler<GetPaginatedProductsQuery, PaginatedResponse<Product>>
    {
        private readonly AppDbContext _context;

        public GetPaginatedProductsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponse<Product>> Handle(GetPaginatedProductsQuery request, CancellationToken cancellationToken)
        {
            // Validation for page and size parameters
            if (request.Page <= 0 || request.Size <= 0)
            {
                throw new ArgumentException("Page and size parameters must be greater than 0.");
            }

            var totalProducts = await _context.Products.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalProducts / request.Size);

            if (request.Page > totalPages)
            {
                return new PaginatedResponse<Product>(totalProducts, totalPages, request.Page, request.Size, new List<Product>());
            }

            var products = await _context.Products
                .Skip((request.Page - 1) * request.Size)
                .Take(request.Size)
                .ToListAsync(cancellationToken);

            return new PaginatedResponse<Product>(
                totalProducts: totalProducts,
                totalPages: totalPages,
                currentPage: request.Page,
                pageSize: request.Size,
                data: products
            );
        }
    }
}
