using ClipNchic.Business.Services;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ClipNchic.Api.Controllers;

[ApiController]
[Route("Charm")]
public class CharmController : ControllerBase
{
    private readonly CharmService _service;
    public CharmController(CharmService service) => _service = service;

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound(new { message = "Charm not found" });
        return Ok(entity);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CharmCreateDto dto)
    {
        var result = await _service.AddAsync(dto);
        if (result > 0) return Ok(new { message = "Charm created successfully" });
        return BadRequest(new { message = "Failed to create Charm" });
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] Charm charm)
    {
        var result = await _service.UpdateAsync(charm);
        if (result > 0) return Ok(new { message = "Charm updated successfully" });
        return BadRequest(new { message = "Failed to update Charm" });
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (result > 0) return Ok(new { message = "Charm deleted successfully" });
        return NotFound(new { message = "Charm not found" });
    }
}