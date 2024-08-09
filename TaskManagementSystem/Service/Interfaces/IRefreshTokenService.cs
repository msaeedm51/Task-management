using TaskManagementSystem.Database.Models;

namespace TaskManagementSystem.Service.Interfaces
{
    public interface IRefreshTokenService
    {
        Task InvalidateRefreshTokensForUser(User user);

        Task InvalidateRefreshTokensForUser(int userId);

        Task<RefreshTokens> AddRefreshTokenForUser(User user, string refreshToken);
        Task<RefreshTokens?> RefreshToken(string refreshTokenString);
    }
}
