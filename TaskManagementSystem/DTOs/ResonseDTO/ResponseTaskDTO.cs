namespace TaskManagementSystem.DTOs
{
    public class ResponseTaskDTO
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string AssignedToUser { get; set; } = string.Empty;
        public string AssignedByUser { get; set; } = string.Empty;
        public List<string> Documents { get; set; } = new List<string>();
    }
}
