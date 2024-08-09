namespace TaskManagementSystem.Database.Models;

public partial class Tasks
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsCompleted { get; set; }

    public DateTime? DueDate { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? AssignedBy { get; set; }
    public int? AssignedTo { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    public virtual User? AssignedByUser { get; set; }
    public virtual User? AssignedToUser { get; set; }
}
