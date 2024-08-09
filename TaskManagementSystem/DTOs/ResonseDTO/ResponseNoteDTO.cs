namespace TaskManagementSystem.DTOs
{ 
    public class ResponseNoteDTO
    {
        public int NoteId { get; set; }
        public int TaskId { get; set; }
        public string? Content { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; }
    }
}
