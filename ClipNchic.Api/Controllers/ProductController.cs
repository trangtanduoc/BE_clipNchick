using ClipNchic.Business.Services;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ClipNchic.Api.Controllers;

[ApiController]
[Route("Product")]
public class ProductController : ControllerBase
{
    private readonly ProductService _service;
    public ProductController(ProductService service)
    {
        _service = service;
    }

    // GET api/products
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var products = await _service.GetProductsAsync();
        return Ok(products);
    }

    // GET api/products/get/{id}
    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var product = await _service.GetProductAsync(id);
        if (product == null) return NotFound(new { message = "Product not found" });
        return Ok(product);
    }

    // POST api/products/create
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] ProductDto product)
    {
        var result = await _service.CreateProductAsync(product);
        if (result > 0) return Ok(new { message = "Product created successfully" });
        return BadRequest(new { message = "Failed to create product" });
    }

    // PUT api/products/update/{id}
    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Product product)
    {
        if (id != product.id) return BadRequest(new { message = "Product ID mismatch" });

        var result = await _service.UpdateProductAsync(product);
        if (result > 0) return Ok(new { message = "Product updated successfully" });
        return BadRequest(new { message = "Failed to update product" });
    }

    // DELETE api/products/delete/{id}
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteProductAsync(id);
        if (result > 0) return Ok(new { message = "Product deleted successfully" });
        return NotFound(new { message = "Product not found" });
    }
}
