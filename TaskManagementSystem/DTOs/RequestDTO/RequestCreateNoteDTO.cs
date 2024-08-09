using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.DTOs
{
    public class RequestCreateNoteDTO
    {
        [Required]
        public int? TaskId { get; set; }
        [Required]
        public string Note { get; set; } = string.Empty;

    }
}
