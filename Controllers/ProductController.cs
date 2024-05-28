using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using GyFChallenge.Models;
using GyFChallenge.Models.DTOs;
using GyFChallenge.Data;

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
            Console.WriteLine("Add new product.");
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
            product.Stock  = data.Stock;
            product.Category  = data.Category;

            try
            {
                _appDbContext.Products.Add(product);
                await _appDbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(List), new { id = product.Id }, product);

            }
            catch (Exception e) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Console.WriteLine("Delete product.");
            var product = await _appDbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            _appDbContext.Products.Remove(product);
            await _appDbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { message = "Product deleted successfully" });
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDTO data)
        {
            Console.WriteLine("Edit product.");
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
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }


        [HttpGet]
        [Route("budget")]
        public async Task<IActionResult> GetFilteredProducts([FromQuery] int budget)
        {
            Console.WriteLine("Getting list of filtered products.");
            if (budget < 1 || budget > 1000000)
            {
                return BadRequest(new { message = "The budget must be between 1 and 1,000,000." });
            }

            var highestPricedProducts = await _appDbContext.Products
                .GroupBy(p => p.Category)
                .Select(g => g.OrderByDescending(p => p.Price).FirstOrDefault())
                .ToListAsync();

            var combinations = from p1 in highestPricedProducts
                               from p2 in highestPricedProducts
                               where p1.Id != p2.Id && p1.Category != p2.Category
                               select new { Product1 = p1, Product2 = p2 };

            var validCombinations = combinations
                .Where(c => c.Product1.Price + c.Product2.Price <= budget)
                .ToList();

            if (!validCombinations.Any())
            {
                return NotFound(new { message = "No products were found that meet the conditions." });
            }

            var bestCombination = validCombinations
                .OrderBy(c => budget - (c.Product1.Price + c.Product2.Price))
                .FirstOrDefault();

            return Ok(new
            {
                Product1 = bestCombination.Product1,
                Product2 = bestCombination.Product2,
                TotalPrice = bestCombination.Product1.Price + bestCombination.Product2.Price
            });
        }

    }
}
