using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.DTOs
{
    public class RequestLoginDTO
    {
        [Required]
        public string? UserId { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
