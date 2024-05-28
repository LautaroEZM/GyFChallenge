using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using GyFChallenge.Models; 
using GyFChallenge.Data;

namespace GyFChallenge.Controllers
{
    [Route("api/[controller]")]
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
        [Route("list")]
        public async Task<IActionResult> List()
        {
            var productList = await _appDbContext.Products.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new { value = productList});
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            product.CreatedAt = DateOnly.FromDateTime(DateTime.Now); // Establecer la fecha actual al agregar el producto

            _appDbContext.Products.Add(product);
            await _appDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(List), new { id = product.Id }, product);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
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
        [Route("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product updatedProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _appDbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            product.Value = updatedProduct.Value;
            product.Category = updatedProduct.Category;

            await _appDbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { message = "Product updated successfully", product });
        }

    }
}
