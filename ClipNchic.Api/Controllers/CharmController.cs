using ClipNchic.Business.Services;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

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
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] CharmCreateRequest request)
    {
        try
        {
            if (request.ModelFile != null && !request.ModelFile.FileName.ToLower().EndsWith(".glb"))
            {
                return BadRequest(new { message = "Model file must be in GLB format (.glb)" });
            }

            var dto = new CharmCreateDto
            {
                name = request.name,
                price = request.price
            };

            var result = await _service.AddAsync(dto, request.ImageFile, request.ModelFile);
            if (result > 0) return Ok(new { message = "Charm created successfully" });
            return BadRequest(new { message = "Failed to create Charm" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
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

public class CharmCreateRequest
{
    public string? name { get; set; }
    public decimal? price { get; set; }
    public IFormFile? ImageFile { get; set; }
    public IFormFile? ModelFile { get; set; }
}