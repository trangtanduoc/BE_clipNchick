using ClipNchic.Business.Services;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> Create([FromBody] BaseCreateDto dto)
    {
        var result = await _service.AddAsync(dto);
        if (result > 0) return Ok(new { message = "Base created successfully" });
        return BadRequest(new { message = "Failed to create Base" });
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