using ClipNchic.Api.Models;
using ClipNchic.Business.Services;
using ClipNchic.DataAccess.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { message = "Email and password are required." });
        }

        var email = request.Email ?? request.Username;
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest(new { message = "Email is required." });
        }

        var token = await _userService.LoginAndGenerateTokenAsync(email, request.Password);
        if (token == null)
        {
            return Unauthorized(new { message = "Invalid email or password." });
        }

        var user = await _userService.GetUserByEmailAsync(email);
        return Ok(new
        {
            message = "Login successful",
            token,
            user = user != null ? UserProfileDto.FromEntity(user) : null
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (request == null)
        {
            return BadRequest(new { message = "Invalid payload." });
        }

        try
        {
            var user = new User
            {
                name = request.FullName,
                password = request.Password,
                email = request.Email
            };

            await _userService.RegisterUserAsync(user);
            return Ok(new
            {
                message = "Registration successful",
                user = UserProfileDto.FromEntity(user)
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.IdToken))
        {
            return BadRequest(new { message = "Google ID token is required." });
        }

        try
        {
            var result = await _userService.LoginWithGoogleAsync(request.IdToken);
            if (result == null)
            {
                return Unauthorized(new { message = "Unable to authenticate with Google." });
            }

            return Ok(new
            {
                message = "Login successful",
                token = result.Value.Token,
                user = UserProfileDto.FromEntity(result.Value.User)
            });
        }
        catch (InvalidJwtException)
        {
            return Unauthorized(new { message = "Invalid Google token." });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }
}

public class LoginRequest
{
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    public string? FullName { get; set; }
    public string Password { get; set; } = string.Empty;
    public string? Email { get; set; }
}

public class GoogleLoginRequest
{
    public string IdToken { get; set; } = string.Empty;
}
