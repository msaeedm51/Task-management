using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DTOs;
using TaskManagementSystem.MappingExtensionMethods;
using TaskManagementSystem.Service.Interfaces;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize()]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;
        public TeamsController (ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> GetAllTeams()
        {
            try
            {
                var teams = await _teamService.GetAll();
                return Ok(teams.Select(t => t.MapToTeamDTO()));
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while getting teams from the database : {0} : {1}", ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }

        [HttpGet("{teamId}")]
        [Authorize(Roles = "Admin, Manager, Team Leader")]
        public async Task<IActionResult> GetTeamById(int teamId)
        {
            try
            {
                var team = await _teamService.GetById(teamId);
                if (team is null)
                {
                    return NotFound("No Team found");
                }
                return Ok(team.MapToTeamDTO());
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while getting a team with id:{0} from the database : {1} : {2}", teamId, ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }

        
        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> CreateTeam([FromBody] RequestCreateTeamDTO teamToCreate)
        {
            try
            {
                if (teamToCreate is null)
                {
                    return BadRequest();
                }

                var teamDB = await _teamService.AddTeam(teamToCreate);
                return CreatedAtAction(nameof(CreateTeam), new { id = teamDB!.Id }, teamDB.MapToTeamDTO());
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while creating team from the database : {0} : {1}", ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }

        [HttpPut("{teamId}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> UpdateTeam(int teamId, [FromBody] RequestUpdateTeamDTO teamToUpdate)
        {
            try
            {
                if (teamId != teamToUpdate.Id)
                {
                    return BadRequest();
                }

                var updatedTeam = await _teamService.UpdateTeam(teamToUpdate);
                if (updatedTeam is null) return NotFound("No such team found");

                return NoContent();
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while Updating team from the database : {0} : {1}", ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }

        [HttpDelete("{teamId}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteTeam(int teamId)
        {
            try
            {
                var success = await _teamService.DeleteTeam(teamId);
                if (success == false) return NotFound("No Such team found");

                return NoContent();
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while Deleting team from the database : {0} : {1}", ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }
    }
}
