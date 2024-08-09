using TaskManagementSystem.AuthenticationHelper;
using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DatabaseAccessors.Interfaces;
using TaskManagementSystem.Service.Interfaces;

namespace TaskManagementSystem.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenAccessor _refreshTokenAccessor;
        public RefreshTokenService(IRefreshTokenAccessor refreshTokenAccessor) 
        { 
          _refreshTokenAccessor = refreshTokenAccessor;
        }
        public async Task<RefreshTokens> AddRefreshTokenForUser(User user, string refreshToken)
        {
            return await _refreshTokenAccessor.AddRefreshTokenForUser(user.Id, refreshToken);
        }

        public async Task<RefreshTokens?> RefreshToken(string refreshTokenString)
        {
           var refreshToken = await _refreshTokenAccessor.GetRefreshToken(refreshTokenString);
           if (refreshToken == null) { return null; }

            if (PasswordManager.ValidateRefreshToken(refreshToken, refreshTokenString))
            {
                refreshToken.RefreshToken = PasswordManager.GenerateRefreshToken();
                refreshToken.RefreshTokenExpiry = DateTime.Now.AddMinutes(45);
                refreshToken.Active = true;
            }

            return refreshToken;
        }

        public async Task InvalidateRefreshTokensForUser(User user)
        {
            await InvalidateRefreshTokensForUser(user.Id);
        }

        public async Task InvalidateRefreshTokensForUser(int userId)
        {
            await _refreshTokenAccessor.InvalidateRefreshTokensForUser(userId);
        }
    }
}
