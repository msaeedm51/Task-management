using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Database;
using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DatabaseAccessors.Interfaces;

namespace TaskManagementSystem.DatabaseAccessors
{
    public class RoleAccessor : IRoleAccessor
    {
        private readonly TaskManagementDbContext _context;
        public RoleAccessor(TaskManagementDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Roles?>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Roles?> GetByNameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(d=> d.Role.Equals(roleName));
        }
    }
}
