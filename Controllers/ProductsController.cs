using Microsoft.AspNetCore.Mvc;
using EshopApi.Models;
using EshopApi.Data;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpPatch("{id}/description")]
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
    }
}
