using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Database;
using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DatabaseAccessors.Interfaces;

namespace TaskManagementSystem.DatabaseAccessors
{
    public class TeamAccessor : ITeamAccessor
    {
        private readonly TaskManagementDbContext _context;
        public TeamAccessor(TaskManagementDbContext context)
        {
            _context = context;
        }
        public async Task<Team?> AddTeamAsync(Team teamToAdd)
        {
            await _context.Teams.AddAsync(teamToAdd);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(teamToAdd.Id);
        }

        public async Task<bool> DeleteTeamAsync(int id)
        {
            var team = await GetByIdAsync(id);

            if(team is null) return false;
            
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Team>> GetAllAsync()
        {
           return await _context.Teams.AsNoTracking().ToListAsync();
                 
        }

        public async Task<IEnumerable<Team>> GetAllForReportAsync()
        {
            return await _context.Teams
                         .AsNoTracking()
                         .Include(t => t.Users)
                         .ThenInclude(t => t.AssignedToTasks)
                         .ToListAsync();
        }

        public async Task<Team?> GetTeamForReportAsync(int teamId)
        {
            return await _context.Teams
                         .AsNoTracking()
                         .Include(t => t.Users)
                         .ThenInclude(t => t.AssignedToTasks)
                         .FirstOrDefaultAsync(t => t.Id == teamId);
        }

        public async Task<Team?> GetByIdAsync(int id)
        {
            return await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Team?> UpdateTeamAsync(Team team)
        {
            var teamDB = await GetByIdAsync(team.Id);
            if (teamDB is null) return null;

            _context.Update(team);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(team.Id);

        }
    }
}
