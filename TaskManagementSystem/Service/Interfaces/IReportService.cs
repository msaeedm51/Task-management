using TaskManagementSystem.Database.Models;

namespace TaskManagementSystem.Service.Interfaces
{
    public interface IReportService
    {
        public Task<IEnumerable<Team>> GetAllTeamsReport();
        public Task<Team?> GetTeamReport(int teamId);
    }
}
