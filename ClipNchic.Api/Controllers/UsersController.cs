using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ClipNchic.Api.Models;
using ClipNchic.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClipNchic.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = GetUserIdFromClaims();
        if (userId == null)
        {
            return Unauthorized(new { message = "User identifier is not present in the token." });
        }

        var user = await _userService.GetUserByIdAsync(userId.Value);
        if (user == null)
        {
            return NotFound(new { message = "User not found." });
        }

        return Ok(UserProfileDto.FromEntity(user));
    }

    [HttpPut("me")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateCurrentUser([FromBody] UpdateUserRequest request)
    {
        if (request == null)
        {
            return BadRequest(new { message = "Invalid payload." });
        }

        var userId = GetUserIdFromClaims();
        if (userId == null)
        {
            return Unauthorized(new { message = "User identifier is not present in the token." });
        }

        var user = await _userService.UpdateUserProfileAsync(
            userId.Value,
            request.Name,
            request.Phone,
            request.Birthday,
            request.Address,
            request.Image);

        if (user == null)
        {
            return NotFound(new { message = "User not found." });
        }

        return Ok(UserProfileDto.FromEntity(user));
    }

    [HttpDelete("me/delete_picture")]
    public async Task<IActionResult> DeleteProfilePicture()
    {
        var userId = GetUserIdFromClaims();
        if (userId == null)
        {
            return Unauthorized(new { message = "User identifier is not present in the token." });
        }

        var user = await _userService.GetUserByIdAsync(userId.Value);
        if (user == null)
        {
            return NotFound(new { message = "User not found." });
        }
        var result = await _userService.UpdateUserProfileAsync(
            user.id,
            user.name,
            user.phone,
            user.birthday,
            user.address,
            null);
        if (result == null)
        {
            return NotFound(new { message = "User not found." });
        }
        return Ok(UserProfileDto.FromEntity(result));
    }

    private int? GetUserIdFromClaims()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst(JwtRegisteredClaimNames.Sub);
        return idClaim != null && int.TryParse(idClaim.Value, out var userId) ? userId : null;
    }
}

public class UpdateUserRequest
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public DateTime? Birthday { get; set; }
    public string? Address { get; set; }
    public IFormFile? Image { get; set; }
}
