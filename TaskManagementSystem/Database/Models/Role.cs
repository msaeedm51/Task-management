namespace TaskManagementSystem.Database.Models;

public partial class Roles
{
    public int Id { get; set; }

    public string Role { get; set; } = null!;

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
