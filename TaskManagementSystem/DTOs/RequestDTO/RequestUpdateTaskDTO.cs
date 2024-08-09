using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.DTOs.RequestDTO
{
    public class RequestUpdateTaskDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CompletedDate { get; set; }

        public int? AssignedTo { get; set; }

        public IEnumerable<IFormFile>? DocumentFiles { get; set; }
    }
}
