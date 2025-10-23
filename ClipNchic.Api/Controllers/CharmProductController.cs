using ClipNchic.Business.Services;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ClipNchic.Api.Controllers;

[ApiController]
[Route("CharmProduct")]
public class CharmProductController : ControllerBase
{
    private readonly CharmProductService _service;
    public CharmProductController(CharmProductService service) => _service = service;

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound(new { message = "CharmProduct not found" });
        return Ok(entity);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CharmProductCreateDto dto)
    {
        var result = await _service.AddAsync(dto);
        if (result > 0) return Ok(new { message = "CharmProduct created successfully" });
        return BadRequest(new { message = "Failed to create CharmProduct" });
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] CharmProduct charmProduct)
    {
        var result = await _service.UpdateAsync(charmProduct);
        if (result > 0) return Ok(new { message = "CharmProduct updated successfully" });
        return BadRequest(new { message = "Failed to update CharmProduct" });
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (result > 0) return Ok(new { message = "CharmProduct deleted successfully" });
        return NotFound(new { message = "CharmProduct not found" });
    }
}