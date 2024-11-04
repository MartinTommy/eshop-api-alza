using EshopApiAlza.Infrastructure.Data;
using MediatR;

namespace EshopApiAlza.Application.Commands
{
    public class UpdateProductDescriptionCommandHandler : IRequestHandler<UpdateProductDescriptionCommand, bool>
    {
        private readonly AppDbContext _context;

        public UpdateProductDescriptionCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateProductDescriptionCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null) return false;

            product.Description = request.NewDescription;
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }

}
