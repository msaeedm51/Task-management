using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DTOs;

namespace TaskManagementSystem.Service.Interfaces
{
    public interface ITeamService
    {
        Task<IEnumerable<Team>> GetAll();
        Task<Team?> GetById(int teamId);
        Task<Team?> AddTeam(RequestCreateTeamDTO teamToAdd);
        Task<Team?> UpdateTeam(RequestUpdateTeamDTO teamToUpdate);
        Task<bool> DeleteTeam(int teamId);
    }
}
