using ClipNchic.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;

namespace ClipNchic.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    public AuthController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email == request.Username && u.PasswordHash == request.Password);
        if (user == null)
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }
        return Ok(new { message = "Login successful", userId = user.UserId, email = user.Email, fullName = user.FullName });
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}