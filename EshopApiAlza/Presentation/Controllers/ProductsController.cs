using Microsoft.AspNetCore.Mvc;
using EshopApiAlza.Domain.Models;
using MediatR;
using EshopApiAlza.Application.Queries;
using EshopApiAlza.Application.Commands;

namespace EshopApiAlza.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/products 
        /// <summary>
        /// Retrieves a list of all products.
        /// </summary>
        /// <returns>A list of products.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }

        // PATCH: api/products/{id}/description
        /// <summary>
        /// Updates description of a specific product selected by its id.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="newDescription">The string to be updated into products description.</param>
        [HttpPatch("{id}/description")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProductDescription(int id, [FromBody] string newDescription)
        {
            var result = await _mediator.Send(new UpdateProductDescriptionCommand(id, newDescription));
            return result ? NoContent() : NotFound();
        }

        // GET: api/products/{id}
        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>The product with the specified ID.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
 
            var product = await _mediator.Send(new GetProductByIdQuery(id));

            if (product == null)
            {
                return NotFound(new { message = $"Product with Id {id} not found." });
            }

            return Ok(product);
            
        }
    }
}
