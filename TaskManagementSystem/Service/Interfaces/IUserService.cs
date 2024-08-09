using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DTOs;

namespace TaskManagementSystem.Service.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetByEmail(string email);
        Task<User?> GetById(int id);
        Task<IEnumerable<User?>> GetUsersByTeam(int teamId);
        Task<IEnumerable<User>> GetAll();
        Task<User?> AddUser(RequestCreateUserDTO userToCreate);
        Task<User?> UpdateUser(RequestUpdateUserDTO userToUpdate);
        Task<bool> DeleteUser(int id);
    }
}
