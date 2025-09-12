using ClipNchic.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClipNchic.DataAccess.Models;

namespace ClipNchic.DataAccess.Repositories
{
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
            return await _context.SaveChangesAsync();
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await Task.FromResult(_context.Users.FirstOrDefault(u => u.Email == email));
        }
        public async Task<User?> LoginAsync(string email, string password)
        {
            return await Task.FromResult(_context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password));
        }
    }
}
