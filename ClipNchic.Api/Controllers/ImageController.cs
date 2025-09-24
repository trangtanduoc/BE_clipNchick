using ClipNchic.Business.Services;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ClipNchic.Api.Controllers;

[ApiController]
[Route("Image")]
public class ImageController : ControllerBase
{
    private readonly ImageService _service;
    public ImageController(ImageService service) => _service = service;

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound(new { message = "Image not found" });
        return Ok(entity);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] ImageCreateDto dto)
    {
        var result = await _service.AddAsync(dto);
        if (result > 0) return Ok(new { message = "Image created successfully" });
        return BadRequest(new { message = "Failed to create Image" });
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] Image image)
    {
        var result = await _service.UpdateAsync(image);
        if (result > 0) return Ok(new { message = "Image updated successfully" });
        return BadRequest(new { message = "Failed to update Image" });
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (result > 0) return Ok(new { message = "Image deleted successfully" });
        return NotFound(new { message = "Image not found" });
    }
}