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
    public ProductController(ProductService service) => _service = service;

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound(new { message = "Product not found" });
        return Ok(entity);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
    {
        var result = await _service.AddAsync(dto);
        if (result > 0) return Ok(new { message = "Product created successfully" });
        return BadRequest(new { message = "Failed to create Product" });
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] ProductUpdateDto dto)
    {
        var result = await _service.UpdateAsync(dto);
        if (result > 0) return Ok(new { message = "Product updated successfully" });
        return BadRequest(new { message = "Failed to update Product" });
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (result > 0) return Ok(new { message = "Product deleted successfully" });
        return NotFound(new { message = "Product not found" });
    }
}