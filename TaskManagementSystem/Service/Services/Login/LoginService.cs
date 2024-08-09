using TaskManagementSystem.AuthenticationHelper;
using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DTOs;
using TaskManagementSystem.Service.Interfaces;

namespace TaskManagementSystem.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserService _userService;
        private readonly IRefreshTokenService _refreshTokenService;

        public LoginService(IUserService userService,
                            IRefreshTokenService refreshTokenService)
        {
            _userService = userService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<User?> Login(RequestLoginDTO userLogin)
        {
            var user = await _userService.GetByEmail(userLogin.UserId!);

            if (user == null) { return null; }

            bool result = PasswordManager.ValidatePassword(userLogin.Password!, user.Salt, user.Password);

            if (!result) { return null; }

            return user;
        }

        public async Task<bool> Logout(RequestRefreshTokenDTO refreshToken)
        {
            var refreshTokenDB = await _refreshTokenService.RefreshToken(refreshToken.RefreshToken);

            if (refreshTokenDB == null) return false;

            if (PasswordManager.ValidateRefreshToken(refreshTokenDB, refreshToken.RefreshToken))
            {
                await _refreshTokenService.InvalidateRefreshTokensForUser(refreshTokenDB.UserId);
            }

            return true;
        }

    }
}
