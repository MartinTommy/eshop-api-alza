using EshopApiAlza.Domain.Models;
using EshopApiAlza.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EshopApiAlza.Application.Queries
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
    {
        private readonly AppDbContext _context;

        public GetAllProductsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products.ToListAsync(cancellationToken);
        }
    }

}
