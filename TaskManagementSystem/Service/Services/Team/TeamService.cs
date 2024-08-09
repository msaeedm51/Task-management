using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DatabaseAccessors.Interfaces;
using TaskManagementSystem.DTOs;
using TaskManagementSystem.Service.Interfaces;

namespace TaskManagementSystem.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamAccessor _teamAccessor;
        private readonly IUserAccessor _userAccessor;

        public TeamService(ITeamAccessor teamAccessor, IUserAccessor userAccessor)
        {
            _teamAccessor = teamAccessor;
            _userAccessor = userAccessor;
        }

        public async Task<Team?> AddTeam(RequestCreateTeamDTO teamToAdd)
        {
            if(teamToAdd is null) return null;

            Team team = new Team()
            {
                Name = teamToAdd.TeamName
            };

            Team? teamCreated = await _teamAccessor.AddTeamAsync(team);

            return teamCreated;
        }

        public async Task<bool> DeleteTeam(int teamId)
        {
            IEnumerable<User>? Users = await  _userAccessor.GetByTeamIdAsync(teamId);
            if (Users is not null)
            {
                foreach (var user in Users)
                {
                    user.TeamId = null;
                    await _userAccessor.UpdateUserAsync(user);
                }
            }
            return await _teamAccessor.DeleteTeamAsync(teamId);
        }

        public async Task<IEnumerable<Team>> GetAll()
        {
            return await _teamAccessor.GetAllAsync();
        }

        public async Task<Team?> GetById(int teamId)
        {
            return await _teamAccessor.GetByIdAsync(teamId);
        }

        public async Task<Team?> UpdateTeam(RequestUpdateTeamDTO teamToUpdate)
        {
            if(teamToUpdate is null) return null;

            Team team = new Team()
            {
                Id = teamToUpdate.Id ?? 0,
                Name = teamToUpdate.TeamName
            };

            Team? teamUpdated = await _teamAccessor.UpdateTeamAsync(team);

            return teamUpdated;
        }
    }
}
