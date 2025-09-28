using ClipNchic.Business.Services;
using Microsoft.AspNetCore.Mvc;
using ClipNchic.DataAccess.Models;

namespace ClipNchic.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var token = _userService.LoginAndGenerateTokenAsync(request.Username, request.Password);
        if (token.Result == null)
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }
        return Ok(new { message = "Login successful", token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var user = new User
            {
                name = request.FullName,
                password = request.Password,
                email = request.Email
            };
            var userId = await _userService.RegisterUserAsync(user);
            return Ok(new { message = "Registration successful", userId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
public class RegisterRequest
{
    public string FullName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}