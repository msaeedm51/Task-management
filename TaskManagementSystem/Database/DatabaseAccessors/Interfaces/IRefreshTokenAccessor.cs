using TaskManagementSystem.Database.Models;

namespace TaskManagementSystem.DatabaseAccessors.Interfaces
{
    public interface IRefreshTokenAccessor
    {
        Task InvalidateRefreshTokensForUser(int userId);
        Task<RefreshTokens> AddRefreshTokenForUser(int userId, string refreshToken);
        Task<RefreshTokens?> GetRefreshToken(string refreshTokenString);
    }
}
