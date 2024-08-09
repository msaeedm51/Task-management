namespace TaskManagementSystem.Database.Models;

public partial class Document
{
    public int Id { get; set; }

    public string? FilePath { get; set; }

    public string? FileName { get; set; }

    public int? TaskId { get; set; }

    public virtual Tasks? Task { get; set; }
}
