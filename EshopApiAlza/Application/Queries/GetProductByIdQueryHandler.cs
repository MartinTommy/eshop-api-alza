using EshopApiAlza.Domain.Models;
using EshopApiAlza.Infrastructure.Data;
using MediatR;

namespace EshopApiAlza.Application.Queries
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product?>
    {
        private readonly AppDbContext _context;

        public GetProductByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.ProductId,
         cancellationToken);

            return product;
        }
    }

}
