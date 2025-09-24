using ClipNchic.Business.Services;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ClipNchic.Api.Controllers;

[ApiController]
[Route("BlindBox")]
public class BlindBoxController : ControllerBase
{
    private readonly BlindBoxService _service;
    public BlindBoxController(BlindBoxService service) => _service = service;

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound(new { message = "BlindBox not found" });
        return Ok(entity);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] BlindBoxCreateDto dto)
    {
        var result = await _service.AddAsync(dto);
        if (result > 0) return Ok(new { message = "BlindBox created successfully" });
        return BadRequest(new { message = "Failed to create BlindBox" });
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] BlindBox box)
    {
        var result = await _service.UpdateAsync(box);
        if (result > 0) return Ok(new { message = "BlindBox updated successfully" });
        return BadRequest(new { message = "Failed to update BlindBox" });
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (result > 0) return Ok(new { message = "BlindBox deleted successfully" });
        return NotFound(new { message = "BlindBox not found" });
    }
}