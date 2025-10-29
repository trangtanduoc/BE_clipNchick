using ClipNchic.Business.Services;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ClipNchic.Api.Controllers;

[ApiController]
[Route("Product")]
public class ProductController : ControllerBase
{
    private readonly ProductService _service;
    public ProductController(ProductService service) => _service = service;

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound(new { message = "Product not found" });
        return Ok(entity);
    }

    [HttpGet("GetByUserId")]
    [Authorize]
    public async Task<IActionResult> GetByUserId()
    {
        var userId = GetUserIdFromClaims();
        if (userId == null)
        {
            return Unauthorized(new { message = "User identifier is not present in the token." });
        }

        var products = await _service.GetByUserIdAsync(userId.Value);
        if (products == null || !products.Any()) return NotFound(new { message = "No products found for this user" });
        return Ok(products);
    }

    [HttpPost("Create")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
    {
        try
        {
            if (request.ModelFile != null && !request.ModelFile.FileName.ToLower().EndsWith(".json"))
            {
                return BadRequest(new { message = "Model file must be in JSON format (.json)" });
            }

            var dto = new ProductCreateDto
            {
                collectId = request.collectId,
                title = request.title,
                descript = request.descript,
                baseId = request.baseId,
                price = request.price,
                userId = request.userId,
                stock = request.stock,
                createDate = request.createDate,
                status = request.status
            };

            var result = await _service.AddAsync(dto, request.Images, request.ModelFile);
            if (result != null) return Ok(result);
            return BadRequest(new { message = "Failed to create Product" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] ProductUpdateDto dto)
    {
        var result = await _service.UpdateAsync(dto);
        if (result > 0) return Ok(new { message = "Product updated successfully" });
        return BadRequest(new { message = "Failed to update Product" });
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (result > 0) return Ok(new { message = "Product deleted successfully" });
        return NotFound(new { message = "Product not found" });
    }

    private int? GetUserIdFromClaims()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst(JwtRegisteredClaimNames.Sub);
        return idClaim != null && int.TryParse(idClaim.Value, out var userId) ? userId : null;
    }
}

public class ProductCreateRequest
{
    public int? collectId { get; set; }
    public string? title { get; set; }
    public string? descript { get; set; }
    public int? baseId { get; set; }
    public decimal? price { get; set; }
    public int? userId { get; set; }
    public int? stock { get; set; }
    public DateTime? createDate { get; set; }
    public string? status { get; set; }
    public List<IFormFile>? Images { get; set; }
    public IFormFile? ModelFile { get; set; }
}
