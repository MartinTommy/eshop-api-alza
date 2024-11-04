using Microsoft.AspNetCore.Mvc;
using EshopApiAlza.Domain.Models;
using MediatR;
using EshopApiAlza.Application.Queries;
using EshopApiAlza.Application.Responses;

namespace EshopApiAlza.Presentation.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    [ApiExplorerSettings(GroupName = "v2")]
    public class ProductsV2Controller : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsV2Controller(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/v2/products?page=1&size=10
        /// <summary>
        /// Retrieves a list of products on a specific page divided into pages depending on page size.
        /// </summary>
        /// <param name="page"> Number of page to display. </param>
        /// <param name="size"> Size of a one page. </param>
        /// <returns> List of products on specific page</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaginatedResponse<Product>>> GetProducts([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            try
            {
                var response = await _mediator.Send(new GetPaginatedProductsQuery(page, size));
                if (!response.Data.Any())
                {
                    return NotFound(new { message = "No products found for the specified page." });
                }
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
