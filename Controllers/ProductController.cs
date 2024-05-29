using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using GyFChallenge.Models;
using GyFChallenge.Models.DTOs;
using GyFChallenge.Data;
using Newtonsoft.Json;

namespace GyFChallenge.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDBContext _appDbContext;
        public ProductController(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> List()
        {
            Console.WriteLine("Getting list of products.");

            var productList = await _appDbContext.Products.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, productList);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Add([FromBody] ProductDTO data)
        {
            Console.WriteLine($"(Add) Add new product. \r\n{JsonConvert.SerializeObject(data)}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (data.Price <= 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Missing Price." });
            }
            if (data.Stock < 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Missing Stock." });
            }

            var product = new Product();

            product.Name = data.Name;
            product.Price = data.Price;
            product.Stock = data.Stock;
            product.Category = data.Category;
            product.CreatedAt = DateOnly.FromDateTime(DateTime.Now);

            try
            {
                _appDbContext.Products.Add(product);
                await _appDbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(List), new { id = product.Id }, product);

            }
            catch (Exception e)
            {
                Console.WriteLine($"(Add) Error ocurred while creating a product. \r\n{JsonConvert.SerializeObject(new { product })}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Console.WriteLine($"(Delete) Delete product. \r\n{JsonConvert.SerializeObject(new { id })}");
            var product = await _appDbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            _appDbContext.Products.Remove(product);
            await _appDbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { message = "Product deleted successfully" });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Console.WriteLine($"(Get) Get specific product. \r\n{JsonConvert.SerializeObject(new { id })}");
            var product = await _appDbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            return StatusCode(StatusCodes.Status200OK, product);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDTO data)
        {
            Console.WriteLine($"(Update) Edit product. \r\n{JsonConvert.SerializeObject(data)}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _appDbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            product.Name = data.Name;
            product.Price = data.Price;
            product.Stock = data.Stock;
            product.Category = data.Category;

            try
            {
                await _appDbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { message = "Product updated successfully", product });
            }
            catch (Exception e)
            {
                Console.WriteLine($"(Update) Error ocurred while updating a product. \r\n{JsonConvert.SerializeObject(new { product })}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [HttpGet]
        [Route("budget")]
        public async Task<IActionResult> GetFilteredProducts([FromQuery] int budget)
        {
            Console.WriteLine($"(Budget) Get products within budget. \r\n{JsonConvert.SerializeObject(new { budget })}");
            if (budget < 1 || budget > 1000000)
            {
                return BadRequest(new { message = "The budget must be between 1 and 1,000,000." });
            }

            // Obtener los productos con el precio más alto de cada categoría.
            var highestPricedProducts = await _appDbContext.Products
                .GroupBy(p => p.Category)
                .Select(g => g.OrderByDescending(p => p.Price).FirstOrDefault())
                .ToListAsync();

            // Generar combinaciones de productos de diferentes categorías.
            var combinations = from p1 in highestPricedProducts
                               from p2 in highestPricedProducts
                               where p1.Id != p2.Id && p1.Category != p2.Category
                               select new { Product1 = p1, Product2 = p2 };

            // Filtrar las combinaciones que están dentro del presupuesto.
            var validCombinations = combinations
                .Where(c => c.Product1.Price + c.Product2.Price <= budget)
                .ToList();

            if (!validCombinations.Any())
            {
                return NotFound(new { message = "No products were found that meet the conditions." });
            }

            // Seleccionar la mejor combinación que esté más cercana al presupuesto.
            var bestCombination = validCombinations
                .OrderBy(c => budget - (c.Product1.Price + c.Product2.Price))
                .FirstOrDefault();
            var response = new[] { bestCombination.Product1, bestCombination.Product2 };

            return Ok(response);
        }
    }
}
