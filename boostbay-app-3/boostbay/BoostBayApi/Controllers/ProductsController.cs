using BoostBayApi.Data;
using BoostBayApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BoostBayApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            var category = await _context.Categories.FindAsync(product.CategoryId);
            if (category == null)
            {
                return BadRequest("Nincs ilyen kategória!");
            }
            product.Category = null;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            var van = await _context.Products.AnyAsync(p => p.Id == id);
            if (!van)
            {
                return NotFound();
            }
            product.Category = null;
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // keresés a névben
        [HttpGet("search/{text}")]
        public async Task<ActionResult<List<Product>>> Search(string text)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Name.Contains(text))
                .ToListAsync();
        }

        // készlet növelése
        [HttpPut("{id}/restock/{amount}")]
        public async Task<ActionResult<Product>> Restock(int id, int amount)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            if (amount <= 0)
            {
                return BadRequest("A mennyiség legyen nagyobb mint 0!");
            }
            product.Stock = product.Stock + amount;
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
