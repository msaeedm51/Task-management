using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Database;
using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DatabaseAccessors.Interfaces;

namespace TaskManagementSystem.DatabaseAccessors;
public class UserAccessor : IUserAccessor
{
    private readonly TaskManagementDbContext _context;

    public UserAccessor(TaskManagementDbContext context)
    { 
       _context = context;
    }

    public async Task<User?> AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null) return false;

        user.Active = false;
        user.InactivatedDate = DateTime.UtcNow;
        _context.Update(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .Include(u => u.UserRoles)
             .ThenInclude(u => u.Role)
            .ToListAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(d => d.EmailAddress == email);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
                 .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<IEnumerable<User>> GetByTeamIdAsync(int id)
    {
        return await _context.Users
                 .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .Where(d => d.TeamId == id)
                .ToListAsync();
    }

    public async Task<User?> UpdateUserAsync(User user)
    {
        var userDB = await GetByIdAsync(user.Id);
        if (userDB is null) return null;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }
}

