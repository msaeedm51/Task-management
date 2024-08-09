using TaskManagementSystem.Database.Models;

namespace TaskManagementSystem.DatabaseAccessors.Interfaces
{
    public interface IRoleAccessor
    {
        Task<Roles?> GetByNameAsync(string roleName);
        Task<IEnumerable<Roles?>> GetAllRolesAsync();
    }
}
