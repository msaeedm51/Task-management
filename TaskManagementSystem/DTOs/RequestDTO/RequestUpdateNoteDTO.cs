using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.DTOs
{
    public class RequestUpdateNoteDTO
    {
        [Required]
        public int NoteId { get; set; }
        [Required]
        public int? TaskId { get; set; }
        [Required]
        public string Note { get; set; } = string.Empty;
    }
}
