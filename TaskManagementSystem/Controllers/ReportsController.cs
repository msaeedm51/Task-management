using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.MappingExtensionMethods;
using TaskManagementSystem.Service.Interfaces;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("teams")]
        public async Task<ActionResult> GetTeamsReports()
        {
            var teams = await _reportService.GetAllTeamsReport();

            if (teams is null) return NotFound();

            var reports = teams.Select(team => team.MapToReportDTO()).ToList();

            return Ok(reports);
        }

        [HttpGet("team/{teamid}")]
        public async Task<ActionResult> GetTeamReports(int teamId)
        {
            if(teamId == 0) return BadRequest();

            var team = await _reportService.GetTeamReport(teamId);

            if (team is null) return NotFound();


            var report = team.MapToReportDTO();

            return Ok(report);
        }
    }
}
