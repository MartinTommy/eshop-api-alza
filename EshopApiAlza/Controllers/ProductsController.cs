using Microsoft.AspNetCore.Mvc;
using EshopApiAlza.Models;
using EshopApiAlza.Data;
using Microsoft.EntityFrameworkCore;

namespace EshopApiAlza.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
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
            return await _context.Products.ToListAsync();
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
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound(new { message = $"Product with Id {id} not found." }); // 404 Not Found
            }

            product.Description = newDescription;
            await _context.SaveChangesAsync();

            return NoContent();  // 204 No Content
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
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound(new { message = $"Product with Id {id} not found." });
            }

            return Ok(product);
        }
    }
}
