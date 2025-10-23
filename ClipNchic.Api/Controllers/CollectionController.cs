using ClipNchic.Business.Services;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ClipNchic.Api.Controllers;

[ApiController]
[Route("Collection")]
public class CollectionController : ControllerBase
{
    private readonly CollectionService _service;
    public CollectionController(CollectionService service) => _service = service;

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound(new { message = "Collection not found" });
        return Ok(entity);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CollectionCreateDto dto)
    {
        var result = await _service.AddAsync(dto);
        if (result > 0) return Ok(new { message = "Collection created successfully" });
        return BadRequest(new { message = "Failed to create Collection" });
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] Collection collection)
    {
        var result = await _service.UpdateAsync(collection);
        if (result > 0) return Ok(new { message = "Collection updated successfully" });
        return BadRequest(new { message = "Failed to update Collection" });
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (result > 0) return Ok(new { message = "Collection deleted successfully" });
        return NotFound(new { message = "Collection not found" });
    }
}