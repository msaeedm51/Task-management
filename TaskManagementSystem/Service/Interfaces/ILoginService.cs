using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DTOs;

namespace TaskManagementSystem.Service.Interfaces
{
    public interface ILoginService
    {
        Task<User?> Login(RequestLoginDTO userLogin);
        Task<bool> Logout(RequestRefreshTokenDTO refreshToken);
        
    }
}
