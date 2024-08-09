using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.DTOs
{
    public class RequestRefreshTokenDTO
    {
        [Required]
        public string RefreshToken { get; set; } = "";
    }
}
