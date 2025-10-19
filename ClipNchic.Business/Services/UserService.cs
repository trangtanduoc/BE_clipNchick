using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Repositories;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using System.Net;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace ClipNchic.Business.Services;

public class UserService
{
    private readonly UserRepo _userRepo;
    private readonly string? _jwtKey;
    private readonly string? _jwtIssuer;
    private readonly string? _googleClientId;
    private readonly Cloudinary _cloudinary;

    public UserService(UserRepo userRepo, IConfiguration configuration, Cloudinary cloudinary)
    {
        _userRepo = userRepo;
        _jwtKey = configuration["Jwt:Key"];
        _jwtIssuer = configuration["Jwt:Issuer"];
        _googleClientId = configuration["Google:ClientId"];
        _cloudinary = cloudinary;
    }

    public async Task<int> RegisterUserAsync(User user)
    {
        if (string.IsNullOrWhiteSpace(user.email))
        {
            throw new InvalidOperationException("Email is required for registration.");
        }

        var existingUser = await _userRepo.GetUserByEmailAsync(user.email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        user.createDate = DateTime.UtcNow;
        user.status ??= "Active";
        return await _userRepo.AddUserAsync(user);
    }

    public async Task<string?> LoginAndGenerateTokenAsync(string email, string password)
    {
        var user = await _userRepo.LoginAsync(email, password);
        if (user == null)
        {
            return null;
        }

        return GenerateJwtToken(user);
    }

    public async Task<(string Token, User User)?> LoginWithGoogleAsync(string idToken)
    {
        if (string.IsNullOrWhiteSpace(idToken))
        {
            throw new ArgumentException("Google ID token is required.", nameof(idToken));
        }

        if (string.IsNullOrWhiteSpace(_googleClientId))
        {
            throw new InvalidOperationException("Google ClientId is not configured.");
        }

        var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new[] { _googleClientId }
        });

        if (string.IsNullOrWhiteSpace(payload.Email))
        {
            throw new InvalidOperationException("Google account does not provide an email address.");
        }

        var user = await _userRepo.GetUserByEmailAsync(payload.Email);
        if (user == null)
        {
            user = new User
            {
                email = payload.Email,
                name = payload.Name,
                image = payload.Picture,
                createDate = DateTime.UtcNow,
                status = "Active"
            };

            await _userRepo.AddUserAsync(user);
        }
        else
        {
            var updated = false;

            if (!string.IsNullOrWhiteSpace(payload.Name) && !string.Equals(user.name, payload.Name, StringComparison.Ordinal))
            {
                user.name = payload.Name;
                updated = true;
            }

            if (!string.IsNullOrWhiteSpace(payload.Picture) && !string.Equals(user.image, payload.Picture, StringComparison.Ordinal))
            {
                user.image = payload.Picture;
                updated = true;
            }

            if (user.status == null)
            {
                user.status = "Active";
                updated = true;
            }

            if (updated)
            {
                await _userRepo.UpdateUserAsync(user);
            }
        }

        var token = GenerateJwtToken(user);
        return (token, user);
    }

    public Task<User?> GetUserByIdAsync(int userId)
    {
        return _userRepo.GetUserByIdAsync(userId);
    }

    public Task<User?> GetUserByEmailAsync(string email)
    {
        return _userRepo.GetUserByEmailAsync(email);
    }

    public async Task<User?> UpdateUserProfileAsync(
        int userId,
        string? name,
        string? phone,
        DateTime? birthday,
        string? address,
        string? imageUrl,
        IFormFile? imageFile = null,
        bool removeImage = false)
    {
        var user = await _userRepo.GetUserByIdAsync(userId);
        if (user == null)
        {
            return null;
        }

        if (name is not null)
        {
            user.name = name;
        }

        if (phone is not null)
        {
            user.phone = phone;
        }

        if (birthday.HasValue)
        {
            user.birthday = birthday.Value;
        }

        if (address is not null)
        {
            user.address = address;
        }

        if (removeImage)
        {
            user.image = null;
        }
        else
        {
            if (imageUrl is not null)
            {
                user.image = imageUrl;
            }
            else if (imageFile is not null && imageFile.Length > 0)
            {
                var uploadedUrl = await UploadImageAsync(imageFile);
                if (!string.IsNullOrWhiteSpace(uploadedUrl))
                {
                    user.image = uploadedUrl;
                }
            }
        }

        await _userRepo.UpdateUserAsync(user);
        return user;
    }

    private string GenerateJwtToken(User user)
    {
        if (string.IsNullOrWhiteSpace(_jwtKey))
        {
            throw new InvalidOperationException("JWT key is not configured.");
        }

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.email ?? string.Empty),
            new(ClaimTypes.NameIdentifier, user.id.ToString()),
            new(ClaimTypes.DateOfBirth, user.birthday?.ToString("yyyy-MM-dd") ?? string.Empty),
            new(ClaimTypes.StreetAddress, user.address ?? string.Empty),
            new(ClaimTypes.MobilePhone, user.phone ?? string.Empty),
            new(ClaimTypes.Role, "User")
        };

        if (!string.IsNullOrWhiteSpace(user.name))
        {
            claims.Add(new Claim(ClaimTypes.Name, user.name!));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtIssuer,
            audience: _jwtIssuer,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public async Task<string?> UploadImageAsync(IFormFile? file)
    {
        if (file == null || file.Length == 0) return null;

        var tempPath = Path.GetTempFileName();
        await using (var stream = System.IO.File.Create(tempPath))
        {
            await file.CopyToAsync(stream);
        }

        try
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(tempPath)
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.StatusCode == HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }

            return null;
        }
        finally
        {
            if (System.IO.File.Exists(tempPath))
            {
                System.IO.File.Delete(tempPath);
            }
        }
    }
}
