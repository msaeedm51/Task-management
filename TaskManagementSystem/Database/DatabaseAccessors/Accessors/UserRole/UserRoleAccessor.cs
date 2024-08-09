using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Database;
using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DatabaseAccessors.Interfaces;

namespace TaskManagementSystem.DatabaseAccessors
{
    public class UserRoleAccessor : IUserRoleAccessor
    {
        private readonly TaskManagementDbContext _context;
        private readonly IUserAccessor  _userAccessor;

        public UserRoleAccessor(TaskManagementDbContext context,
                                IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
            _context = context;
        }

        public async Task<UserRole?> AddRoleToUserAsync(UserRole userRoleToAdd)
        {
            await _context.UserRoles.AddAsync(userRoleToAdd);
            await _context.SaveChangesAsync();

            return userRoleToAdd;
        }

        public async Task<IEnumerable<UserRole>> GetRolesForUser(User user, bool? active = null)
        {
            if (active is null)
            {
                return await _context.UserRoles
                                 .Include(ur => ur.Role)
                                 .Include(ur => ur.User)
                                 .Where(ur => ur.UserId == user.Id).ToListAsync();

            }
            else
            {
                return await _context.UserRoles
                                 .Include(ur => ur.Role)
                                 .Include(ur => ur.User)
                                .Where(ur => ur.UserId == user.Id && ur.Active == active).ToListAsync();

            }
        }

        public async Task<UserRole?> UpdateUserRole(UserRole userRoleToUpdate)
        {
            User? user = await _userAccessor.GetByIdAsync(userRoleToUpdate.UserId ?? 0);

            if (user is null) return null;

            var roleToUpdate = await GetRolesForUser(user, true);

            UserRole? userRole = roleToUpdate!.Where(r => r.RoleId == userRoleToUpdate.RoleId).FirstOrDefault();

            if (userRole == null) return null;

            userRole.Active = userRoleToUpdate.Active;
            _context.Update(userRole);
            await _context.SaveChangesAsync();
            
            return userRole;
        }
    }
}
