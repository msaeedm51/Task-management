using System.ComponentModel.DataAnnotations;
namespace TaskManagementSystem.DTOs
{
    public class RequestCreateTasksDTO
    {
        [Required(ErrorMessage = "Title Required")]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }
        [Required(ErrorMessage = "UserId Required")]
        public int? AssignedTo { get; set; }

        public IEnumerable<IFormFile>? DocumentFiles { get; set; }
    }
}
