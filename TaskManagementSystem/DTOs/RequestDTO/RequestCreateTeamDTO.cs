using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.DTOs
{
    public class RequestCreateTeamDTO
    {
        [Required]
        public string TeamName { get; set; } = string.Empty;
    }
}
