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

        var (token, error) = await _userService.LoginAndGenerateTokenAsync(email, request.Password);
        if (token == null)
        {
            return Unauthorized(new { message = error ?? "Invalid email or password." });
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

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(new { message = "Email is required." });
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { message = "Password is required." });
        }

        try
        {
            Console.WriteLine($"Register request - Email: {request.Email}, FullName: {request.FullName}, Password length: {request.Password?.Length}");
            
            var user = new User
            {
                name = request.FullName,
                password = request.Password,
                email = request.Email
            };

            var userId = await _userService.RegisterUserAsync(user);

            var emailSent = await _userService.SendVerificationEmailAsync(userId);
            if (!emailSent)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Registration successful, but failed to send verification email." });
            }

            var registeredUser = await _userService.GetUserByIdAsync(userId);
            return Ok(new
            {
                message = "Registration successful. Please check your email to verify your account.",
                user = registeredUser != null ? UserProfileDto.FromEntity(registeredUser) : null
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Token))
        {
            return BadRequest(new { message = "Verification token is required." });
        }

        try
        {
            Console.WriteLine($"Verifying email with token: {request.Token}");
            var result = await _userService.VerifyEmailAsync(request.Token);
            Console.WriteLine($"Email verified successfully!");
            return Ok(new { message = "Email verified successfully!" });
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Invalid operation: {ex.Message}");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error verifying email: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { message = ex.Message });
        }
    }

    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmailGet([FromQuery] string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return Content(GetErrorHtml("Verification token is required."), "text/html");
        }

        try
        {
            Console.WriteLine($"GET: Verifying email with token: {token}");
            var result = await _userService.VerifyEmailAsync(token);
            Console.WriteLine($"Email verified successfully!");
            return Content(GetSuccessHtml(), "text/html");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Invalid operation: {ex.Message}");
            return Content(GetErrorHtml(ex.Message), "text/html");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error verifying email: {ex.Message}");
            return Content(GetErrorHtml("Error verifying email. Please try again."), "text/html");
        }
    }

    private string GetSuccessHtml()
    {
        return @"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Email Verified</title>
    <style>
        * { margin: 0; padding: 0; box-sizing: border-box; }
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
        }
        .container {
            background: white;
            padding: 50px;
            border-radius: 10px;
            box-shadow: 0 10px 40px rgba(0,0,0,0.3);
            text-align: center;
            max-width: 500px;
        }
        .checkmark {
            color: #4CAF50;
            font-size: 80px;
            margin-bottom: 20px;
        }
        h1 { color: #4CAF50; margin-bottom: 10px; }
        p { color: #666; font-size: 16px; margin-bottom: 30px; }
        .button {
            background-color: #4CAF50;
            color: white;
            padding: 12px 30px;
            text-decoration: none;
            border-radius: 5px;
            display: inline-block;
            margin: 10px;
            transition: background-color 0.3s;
        }
        .button:hover { background-color: #45a049; }
        .redirect-message { color: #999; font-size: 12px; margin-top: 20px; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='checkmark'>✓</div>
        <h1>Email Verified Successfully!</h1>
        <p>Your email has been verified. You can now login to your account.</p>
        <a href='/' class='button'>Back to Home</a>
        <p class='redirect-message'>You will be redirected in 5 seconds...</p>
    </div>
    <script>
        setTimeout(() => { window.location.href = '/'; }, 5000);
    </script>
</body>
</html>";
    }

    private string GetErrorHtml(string message)
    {
        return $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Verification Failed</title>
    <style>
        * {{ margin: 0; padding: 0; box-sizing: border-box; }}
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);
            height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
        }}
        .container {{
            background: white;
            padding: 50px;
            border-radius: 10px;
            box-shadow: 0 10px 40px rgba(0,0,0,0.3);
            text-align: center;
            max-width: 500px;
        }}
        .error-icon {{
            color: #f44336;
            font-size: 80px;
            margin-bottom: 20px;
        }}
        h1 {{ color: #f44336; margin-bottom: 10px; }}
        p {{ color: #666; font-size: 16px; margin-bottom: 30px; }}
        .button {{
            background-color: #2196F3;
            color: white;
            padding: 12px 30px;
            text-decoration: none;
            border-radius: 5px;
            display: inline-block;
            margin: 10px;
            transition: background-color 0.3s;
        }}
        .button:hover {{ background-color: #0b7dda; }}
        .error-message {{ 
            background-color: #ffebee;
            color: #c62828;
            padding: 15px;
            border-radius: 5px;
            margin-bottom: 20px;
            word-break: break-word;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='error-icon'>✗</div>
        <h1>Verification Failed</h1>
        <div class='error-message'>{message}</div>
        <p>The verification link may have expired or is invalid.</p>
        <a href='/' class='button'>Back to Home</a>
    </div>
</body>
</html>";
    }

    [HttpPost("resend-verification-email")]
    public async Task<IActionResult> ResendVerificationEmail([FromBody] ResendVerificationRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(new { message = "Email is required." });
        }

        try
        {
            var result = await _userService.ResendVerificationEmailAsync(request.Email);
            return Ok(new { message = "Verification email sent successfully. Please check your inbox." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { message = ex.Message });
        }
    }

    [HttpGet("check-verification-status")]
    public async Task<IActionResult> CheckVerificationStatus([FromQuery] string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest(new { message = "Email is required." });
        }

        try
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            return Ok(new
            {
                email = user.email,
                isVerified = user.isEmailVerified,
                message = user.isEmailVerified ? "Email is verified" : "Email is not yet verified"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { message = ex.Message });
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
    [System.Text.Json.Serialization.JsonPropertyName("fullName")]
    public string? FullName { get; set; }
    
    [System.Text.Json.Serialization.JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
    
    [System.Text.Json.Serialization.JsonPropertyName("email")]
    public string? Email { get; set; }
}

public class GoogleLoginRequest
{
    public string IdToken { get; set; } = string.Empty;
}

public class VerifyEmailRequest
{
    public string Token { get; set; } = string.Empty;
}

public class ResendVerificationRequest
{
    public string Email { get; set; } = string.Empty;
}
