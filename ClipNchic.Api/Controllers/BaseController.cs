using ClipNchic.Business.Services;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ClipNchic.Api.Controllers;

[ApiController]
[Route("Base")]
public class BaseController : ControllerBase
{
    private readonly BaseService _service;
    public BaseController(BaseService service) => _service = service;

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound(new { message = "Base not found" });
        return Ok(entity);
    }

    [HttpPost("Create")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] BaseCreateRequest request)
    {
        try
        {
            var dto = new BaseCreateDto
            {
                name = request.name,
                color = request.color,
                price = request.price
            };

            var result = await _service.AddAsync(dto, request.ImageFile, request.ModelFile);
            if (result > 0) return Ok(new { message = "Base created successfully" });
            return BadRequest(new { message = "Failed to create Base" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] BaseUpdateDto dto)
    {
        var result = await _service.UpdateAsync(dto);
        if (result > 0) return Ok(new { message = "Base updated successfully" });
        return BadRequest(new { message = "Failed to update Base" });
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (result > 0) return Ok(new { message = "Base deleted successfully" });
        return NotFound(new { message = "Base not found" });
    }
}

public class BaseCreateRequest
{
    public string? name { get; set; }
    public string? color { get; set; }
    public decimal? price { get; set; }
    public IFormFile? ImageFile { get; set; }
    public IFormFile? ModelFile { get; set; }
}