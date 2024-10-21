using Microsoft.AspNetCore.Mvc;
using EshopApiAlza.Data;
using EshopApiAlza.Models;
using Microsoft.EntityFrameworkCore;

namespace EshopApiAlza.Controllers
{
    [Route("api/v2/[controller]")]
    [ApiController]
    public class ProductsV2Controller : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsV2Controller(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/v2/products?page=1&size=10
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<Product>>> GetProducts([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            if (page <= 0 || size <= 0)
            {
                return BadRequest(new { message = "Page and size parameters must be greater than 0." });
            }

            var totalProducts = await _context.Products.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalProducts / size);

            if (page > totalPages)
            {
                return NotFound(new { message = "Page number exceeds total pages." });
            }

            var products = await _context.Products
                .Skip((page - 1) * size)  // Skip records for previous pages
                .Take(size)               // Take only the required number of records
                .ToListAsync();

            var response = new PaginatedResponse<Product>(
                totalProducts: totalProducts,
                totalPages: totalPages,
                currentPage: page,
                pageSize: size,
                data: products
            );

            return Ok(response);
        }
    }
}
