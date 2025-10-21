using ClipNchic.DataAccess.Data;
using ClipNchic.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ClipNchic.DataAccess.Repositories;

public class EmailVerificationTokenRepo
{
    private readonly AppDbContext _context;

    public EmailVerificationTokenRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddTokenAsync(EmailVerificationToken token)
    {
        _context.EmailVerificationTokens.Add(token);
        return await _context.SaveChangesAsync();
    }

    public async Task<EmailVerificationToken?> GetTokenByTokenStringAsync(string tokenString)
    {
        return await _context.EmailVerificationTokens
            .FirstOrDefaultAsync(t => t.token == tokenString && !t.isUsed);
    }

    public async Task<EmailVerificationToken?> GetTokenByIdAsync(int tokenId)
    {
        return await _context.EmailVerificationTokens.FindAsync(tokenId);
    }

    public async Task<EmailVerificationToken> UpdateTokenAsync(EmailVerificationToken token)
    {
        _context.EmailVerificationTokens.Update(token);
        await _context.SaveChangesAsync();
        return token;
    }

    public async Task<bool> DeleteExpiredTokensAsync(int userId)
    {
        var expiredTokens = await _context.EmailVerificationTokens
            .Where(t => t.userId == userId && t.expiryDate < DateTime.UtcNow)
            .ToListAsync();

        if (expiredTokens.Count > 0)
        {
            _context.EmailVerificationTokens.RemoveRange(expiredTokens);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<EmailVerificationToken?> GetLatestTokenByUserIdAsync(int userId)
    {
        return await _context.EmailVerificationTokens
            .Where(t => t.userId == userId && !t.isUsed && t.expiryDate > DateTime.UtcNow)
            .OrderByDescending(t => t.createdDate)
            .FirstOrDefaultAsync();
    }
}
