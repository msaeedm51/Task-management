using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.DTOs
{
    public class RequestUpdateTeamDTO
    {
        [Required]
        public int? Id { get; set; }
        [Required]
        public string TeamName { get; set; } = string.Empty;
    }
}
