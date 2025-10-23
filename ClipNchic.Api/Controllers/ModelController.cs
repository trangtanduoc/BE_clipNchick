using ClipNchic.Business.Services;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ClipNchic.Api.Controllers;

[ApiController]
[Route("Model")]
public class ModelController : ControllerBase
{
    private readonly ModelService _service;
    public ModelController(ModelService service) => _service = service;

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound(new { message = "Model not found" });
        return Ok(entity);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] ModelCreateDto dto)
    {
        var result = await _service.AddAsync(dto);
        if (result > 0) return Ok(new { message = "Model created successfully" });
        return BadRequest(new { message = "Failed to create Model" });
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] ModelUpdateDto dto)
    {
        var result = await _service.UpdateAsync(dto);
        if (result > 0) return Ok(new { message = "Model updated successfully" });
        return BadRequest(new { message = "Failed to update Model" });
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (result > 0) return Ok(new { message = "Model deleted successfully" });
        return NotFound(new { message = "Model not found" });
    }
}