namespace TaskManagementSystem.Database.Models;

public partial class UserRole
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? RoleId { get; set; }

    public bool Active { get; set; }

    public virtual Roles? Role { get; set; }

    public virtual User? User { get; set; }
}
