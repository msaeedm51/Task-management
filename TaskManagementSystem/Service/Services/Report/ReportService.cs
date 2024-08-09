using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DatabaseAccessors.Interfaces;
using TaskManagementSystem.Service.Interfaces;

namespace TaskManagementSystem.Services
{
    public class ReportService : IReportService
    {
        private readonly ITeamAccessor _teamAccessor;
        public ReportService(ITeamAccessor teamAccessor)
        {
            _teamAccessor = teamAccessor;
        }
        public async Task<IEnumerable<Team>> GetAllTeamsReport()
        {
            return await _teamAccessor.GetAllForReportAsync();
        }

        public async Task<Team?> GetTeamReport(int teamId)
        {
            return await _teamAccessor.GetTeamForReportAsync(teamId);
        }
    }
}
