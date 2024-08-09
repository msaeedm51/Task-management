namespace TaskManagementSystem.Database.Models;

public partial class Note
{
    public int Id { get; set; }

    public string? Content { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public int? TaskId { get; set; }

    public virtual Tasks? Task { get; set; }
}
