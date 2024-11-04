using EshopApiAlza.Domain.Models;
using MediatR;

namespace EshopApiAlza.Application.Queries
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public int ProductId { get; }

        public GetProductByIdQuery(int productId)
        {
            ProductId = productId;
        }
    }


}
