using ClipNchic.DataAccess.Data;
using ClipNchic.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ClipNchic.DataAccess.Repositories;

public class UserRepo
{
    private readonly AppDbContext _context;

    public UserRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user.id;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.email == email);
    }

    public async Task<User?> LoginAsync(string email, string password)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.email == email && u.password == password);
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }
}
