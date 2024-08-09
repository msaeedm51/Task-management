using TaskManagementSystem.Database.Models;

namespace TaskManagementSystem.DatabaseAccessors.Interfaces
{
    public interface IUserRoleAccessor
    {
        Task<UserRole?> AddRoleToUserAsync(UserRole userRoleToAdd);
        Task<UserRole?> UpdateUserRole(UserRole userRoleToUpdate);

        Task<IEnumerable<UserRole>> GetRolesForUser(User user, bool? active = null);
    }
}
