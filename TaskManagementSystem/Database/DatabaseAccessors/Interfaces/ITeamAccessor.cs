using TaskManagementSystem.Database.Models;

namespace TaskManagementSystem.DatabaseAccessors.Interfaces
{
    public interface ITeamAccessor
    {
        Task<IEnumerable<Team>> GetAllAsync();
        Task<IEnumerable<Team>> GetAllForReportAsync();
        Task<Team?> GetTeamForReportAsync(int teamId);
        Task<Team?> GetByIdAsync(int teamId);
        Task<Team?> AddTeamAsync(Team teamToAdd);
        Task<Team?> UpdateTeamAsync(Team teamToUpdate);
        Task<bool> DeleteTeamAsync(int teamId);
    }
}
