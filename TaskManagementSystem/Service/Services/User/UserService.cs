using TaskManagementSystem.AuthenticationHelper;
using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DatabaseAccessors.Interfaces;
using TaskManagementSystem.DTOs;
using TaskManagementSystem.Service.Interfaces;

namespace TaskManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IRoleAccessor _roleAccessor;
        private readonly IUserRoleAccessor _userRoleAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IUserAccessor  userAccessor,
                           IRoleAccessor roleAccessor,
                           IUserRoleAccessor userRoleAccessor,
                           IHttpContextAccessor httpContextAccessor) 
        { 
            _roleAccessor = roleAccessor;
            _userAccessor = userAccessor;
            _userRoleAccessor = userRoleAccessor;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User?> AddUser(RequestCreateUserDTO userToCreate)
        {
            User? user = new User();
            List<UserRole>? userRoles = new List<UserRole>();

            user.Username = userToCreate.Username;
            user.EmailAddress = userToCreate.EmailAddress;
            user.Salt = PasswordManager.GenerateSalt();
            user.Password = PasswordManager.HashPassword(userToCreate.Password, user.Salt);
            user.Active = true;
            user.CreatedDate = DateTime.Now;
            await _userAccessor.AddUserAsync(user);

            // Convert object to UserDTO to simplify UserRole integration
            UserDTO userDTO = new UserDTO()
            {
                Id = user.Id,
                IsAdmin = userToCreate.IsAdmin,
                IsTeamLead = userToCreate.IsTeamLead,
                IsManager = userToCreate.IsManager
            };

            userRoles = GetUserRolesFromDTO(userDTO);

            if (userRoles is null) return null;


            foreach (UserRole role in userRoles)
            {
                _userRoleAccessor.AddRoleToUserAsync(role).GetAwaiter().GetResult();
            }

            user = _userAccessor.GetByIdAsync(user.Id).GetAwaiter().GetResult();

            if (user is null) return null;

            return user;

        }

        public async Task<bool> DeleteUser(int id)
        {
            return await _userAccessor.DeleteUserAsync(id);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userAccessor.GetAllAsync();
        }

        public async Task<IEnumerable<User?>> GetUsersByTeam(int teamId)
        {
            return await _userAccessor.GetByTeamIdAsync(teamId);
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await _userAccessor.GetByEmailAsync(email);
        }

        public async Task<User?> GetById(int id)
        {
            return await  _userAccessor.GetByIdAsync(id);
        }

        public async Task<User?> UpdateUser(RequestUpdateUserDTO userToUpdate)
        {
            User? user = await _userAccessor.GetByIdAsync(userToUpdate.Id);

            if(user is null) return null;

            UserDTO userDTO = new UserDTO()
            {
                Id = user!.Id,
                IsAdmin = userToUpdate.IsAdmin,
                IsTeamLead = userToUpdate.IsTeamLead,
                IsManager = userToUpdate.IsManager
            };

            var userRoles = GetUserRolesFromDTO(userDTO);

            if (userRoles is null) return null;

            if (user is not null)
            {
                user.Username = userToUpdate.Username;
                user.EmailAddress = userToUpdate.EmailAddress;
                if (userToUpdate.Password.Length > 0)
                {
                    user.Password = PasswordManager.HashPassword(userToUpdate.Password, user.Salt);
                }
                user.Active = userToUpdate.Active;
                user = _userAccessor.UpdateUserAsync(user).GetAwaiter().GetResult();

                var isAdministrator = _httpContextAccessor.HttpContext?.User.IsInRole("Admin");

                if (isAdministrator == true)
                {
                    foreach (UserRole role in userRoles)
                    {
                        await _userRoleAccessor.UpdateUserRole(role);
                    }
                }

                user = _userAccessor.GetByIdAsync(user!.Id).GetAwaiter().GetResult();

            }

            return user;
        }

        private List<UserRole>? GetUserRolesFromDTO(UserDTO userDTO)
        {
            List<UserRole> userRoles = new List<UserRole>();
            IEnumerable<Roles?>? roles = _roleAccessor.GetAllRolesAsync().GetAwaiter().GetResult();

            if (roles is null) return null;

            if (userDTO.IsAdmin)
            {
                userRoles.Add(new UserRole() { UserId = userDTO.Id, RoleId = roles.Where(r => r!.Role == "Admin").Select(r => r!.Id).First(), Active = true });
            }
            else
            {
                userRoles.Add(new UserRole() { UserId = userDTO.Id, RoleId = roles.Where(r => r!.Role == "Admin").Select(r => r!.Id).First(), Active = false });
            }

            if (userDTO.IsManager)
            {
                userRoles.Add(new UserRole() { UserId = userDTO.Id, RoleId = roles.Where(r => r!.Role == "Manager").Select(r => r!.Id).First(), Active = true });
            }
            else
            {
                userRoles.Add(new UserRole() { UserId = userDTO.Id, RoleId = roles.Where(r => r!.Role == "Manager").Select(r => r!.Id).First(), Active = false });
            }

            if (userDTO.IsTeamLead)
            {
                userRoles.Add(new UserRole() { UserId = userDTO.Id, RoleId = roles.Where(r => r!.Role == "Team Leader").Select(r => r!.Id).First(), Active = true });
            }
            else
            {
                userRoles.Add(new UserRole() { UserId = userDTO.Id, RoleId = roles.Where(r => r!.Role == "Team Leader").Select(r => r!.Id).First(), Active = false });
            }


            return userRoles;

        }
    }
}
