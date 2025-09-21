﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using ClipNchic.DataAccess.Repositories;
using ClipNchic.DataAccess.Models;
using Microsoft.Extensions.Configuration;

namespace ClipNchic.Business.Services
{
    public class UserService
    {
        private readonly UserRepo _userRepo;
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;

        public UserService(UserRepo userRepo, IConfiguration configuration)
        {
            _userRepo = userRepo;
            _jwtKey = configuration["Jwt:Key"];
            _jwtIssuer = configuration["Jwt:Issuer"];
        }

        public async Task<int> RegisterUserAsync(User user)
        {
            var existingUser = await _userRepo.GetUserByEmailAsync(user.email);
            if (existingUser != null)
            {
                throw new Exception("User with this email already exists.");
            }
            user.createDate = DateTime.UtcNow;
            return await _userRepo.AddUserAsync(user);
        }

        public async Task<string?> LoginAndGenerateTokenAsync(string email, string password)
        {
            var user = await _userRepo.LoginAsync(email, password);
            if (user == null)
                return null;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.email ?? ""),
                new Claim(ClaimTypes.Role, "User") // Default role since Role table is removed
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtIssuer,
                audience: _jwtIssuer,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
