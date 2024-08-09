using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.DatabaseAccessors.Interfaces;
using TaskManagementSystem.Database.Models;
using TaskManagementSystem.Database;

namespace TaskManagementSystem.DatabaseAccessors
{
    public class RefreshTokenAccessor : IRefreshTokenAccessor
    {
        private readonly TaskManagementDbContext _context;

        public RefreshTokenAccessor(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshTokens> AddRefreshTokenForUser(int userId, string refreshToken)
        {
            RefreshTokens token = new RefreshTokens();
            token.UserId = userId;
            token.RefreshToken = refreshToken;
            token.Active = true;
            token.RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(45);

            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task<RefreshTokens?> GetRefreshToken(string refreshTokenString)
        {
           return  await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.RefreshToken == refreshTokenString);
        }

        public async Task InvalidateRefreshTokensForUser(int userId)
        {
            await _context.RefreshTokens.Where(d => d.UserId == userId && d.Active == true)
                  .ExecuteUpdateAsync(setters => setters
                                      .SetProperty(b => b.Active, false));
        }
    }
}
