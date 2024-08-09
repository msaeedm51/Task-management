using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DTOs;

namespace TaskManagementSystem.MappingExtensionMethods
{
    public static class UserMappingExtension
    {
        public static ResponseUserDTO MapToUserDTO(this User user)
        {
            ResponseUserDTO responseUserDTO = new()
            {
                Id = user.Id,
                Active = user.Active,
                EmailAddress = user.EmailAddress,
                Username = user.Username,
                Roles = user.UserRoles.Where(r=> r.Active).Select(d=> d.Role?.Role).ToList(), 
                Team = user.Team?.Name ?? string.Empty
            };

            return responseUserDTO;

        }
    }
}
