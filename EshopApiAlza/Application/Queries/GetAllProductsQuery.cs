using EshopApiAlza.Domain.Models;
using MediatR;

namespace EshopApiAlza.Application.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<Product>>
    {
    }
}
