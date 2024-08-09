using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.DTOs
{
    public class RequestUpdateUserDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = null!;

        public bool Active { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsManager { get; set; }
        public bool IsTeamLead { get; set; }

        public int? TeamId { get; set; }

        public DateTime? InactivatedDate { get; set; }
    }
}
