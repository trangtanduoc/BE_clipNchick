using ClipNchic.Business.Services;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ClipNchic.Api.Controllers;

[ApiController]
[Route("BlindPic")]
public class BlindPicController : ControllerBase
{
    private readonly BlindPicService _service;
    public BlindPicController(BlindPicService service) => _service = service;

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound(new { message = "BlindPic not found" });
        return Ok(entity);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] BlindPicCreateDto dto)
    {
        var result = await _service.AddAsync(dto);
        if (result > 0) return Ok(new { message = "BlindPic created successfully" });
        return BadRequest(new { message = "Failed to create BlindPic" });
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] BlindPic pic)
    {
        var result = await _service.UpdateAsync(pic);
        if (result > 0) return Ok(new { message = "BlindPic updated successfully" });
        return BadRequest(new { message = "Failed to update BlindPic" });
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (result > 0) return Ok(new { message = "BlindPic deleted successfully" });
        return NotFound(new { message = "BlindPic not found" });
    }
}