using MediatR;

namespace EshopApiAlza.Application.Commands
{
    public class UpdateProductDescriptionCommand : IRequest<bool>
    {
        public int ProductId { get; }
        public string NewDescription { get; }

        public UpdateProductDescriptionCommand(int productId, string newDescription)
        {
            ProductId = productId;
            NewDescription = newDescription;
        }
    }
}
