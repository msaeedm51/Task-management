namespace TaskManagementSystem.Database.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public byte[] Password { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public bool Active { get; set; }

    public DateTime? InactivatedDate { get; set; }

    public byte[] Salt { get; set; } = null!;

    public int? TeamId { get; set; }

    public virtual ICollection<Tasks> AssignedToTasks { get; set; } = new List<Tasks>();

    public virtual ICollection<Tasks> AssignedByTasks { get; set; } = new List<Tasks>();

    public virtual Team? Team { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
