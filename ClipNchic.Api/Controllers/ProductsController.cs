using ClipNchic.Business.Services;
using ClipNchic.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClipNchic.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _service;
        public ProductsController(ProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            var products = await _service.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _service.GetProductAsync(id);
            if (product == null) return NotFound(new { message = "Product not found" });
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            var result = await _service.CreateProductAsync(product);
            if (result > 0) return Ok(new { message = "Product created successfully" });
            return BadRequest(new { message = "Failed to create product" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.ProductId) return BadRequest(new { message = "Product ID mismatch" });

            var result = await _service.UpdateProductAsync(product);
            if (result > 0) return Ok(new { message = "Product updated successfully" });
            return BadRequest(new { message = "Failed to update product" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _service.DeleteProductAsync(id);
            if (result > 0) return Ok(new { message = "Product deleted successfully" });
            return NotFound(new { message = "Product not found" });
        }
    }
}