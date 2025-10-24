using ClipNchic.Business.Services;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ClipNchic.Api.Controllers;

[ApiController]
[Route("Ship")]
public class ShipController : ControllerBase
{
    private readonly ShipService _service;
    public ShipController(ShipService service)
    {
        _service = service;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var ships = await _service.GetAllAsync();
        return Ok(ships);
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var ship = await _service.GetByIdAsync(id);
        if (ship == null) return NotFound(new { message = "Ship not found" });
        return Ok(ship);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] ShipCreateDto dto)
    {
        var result = await _service.AddAsync(dto);
        if (result > 0) return Ok(new { message = "Ship created successfully" });
        return BadRequest(new { message = "Failed to create ship" });
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] Ship ship)
    {
        var result = await _service.UpdateAsync(ship);
        if (result > 0) return Ok(new { message = "Ship updated successfully" });
        return BadRequest(new { message = "Failed to update ship" });
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (result > 0) return Ok(new { message = "Ship deleted successfully" });
        return NotFound(new { message = "Ship not found" });
    }
}