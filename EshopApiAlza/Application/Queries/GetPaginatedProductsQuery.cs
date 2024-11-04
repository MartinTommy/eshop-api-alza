using EshopApiAlza.Application.Responses;
using EshopApiAlza.Domain.Models;
using MediatR;

namespace EshopApiAlza.Application.Queries
{
    public class GetPaginatedProductsQuery : IRequest<PaginatedResponse<Product>>
    {
        public int Page { get; }
        public int Size { get; }

        public GetPaginatedProductsQuery(int page, int size)
        {
            Page = page;
            Size = size;
        }
    }
}
