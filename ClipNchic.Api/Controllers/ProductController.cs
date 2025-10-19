using ClipNchic.Business.Services;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

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
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
    {
        var dto = new ProductCreateDto
        {
            collectId = request.collectId,
            title = request.title,
            descript = request.descript,
            baseId = request.baseId,
            price = request.price,
            userId = request.userId,
            stock = request.stock,
            modelId = request.modelId,
            createDate = request.createDate,
            status = request.status
        };

        var result = await _service.AddAsync(dto, request.Images);
        if (result != null) return Ok(result);
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

public class ProductCreateRequest
{
    public int? collectId { get; set; }
    public string? title { get; set; }
    public string? descript { get; set; }
    public int? baseId { get; set; }
    public decimal? price { get; set; }
    public int? userId { get; set; }
    public int? stock { get; set; }
    public int? modelId { get; set; }
    public DateTime? createDate { get; set; }
    public string? status { get; set; }
    public List<IFormFile>? Images { get; set; }
}
