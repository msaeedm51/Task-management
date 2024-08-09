using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Database.Models;

namespace TaskManagementSystem.Database;

public partial class TaskManagementDbContext : DbContext
{
    public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Roles> Roles { get; set; }

    public virtual DbSet<Tasks> Tasks { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<RefreshTokens> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC0710B1E246");

            entity.ToTable("Document");

            entity.Property(e => e.FileName).HasMaxLength(200);
            entity.Property(e => e.FilePath).HasMaxLength(300);

            entity.HasOne(d => d.Task).WithMany(p => p.Documents)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK__Document__TaskId__440B1D61");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Note__3214EC07F20EC6C2");

            entity.ToTable("Note");

            entity.Property(e => e.Content).HasMaxLength(500); 
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Task).WithMany(p => p.Notes)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK__Note__TaskId__412EB0B6");
        });

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC07A3E2326B");

            entity.ToTable("Roles");

            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("Role");
        });

        modelBuilder.Entity<Tasks>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Task__3214EC07921234F2");

            entity.ToTable("Task");

            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.CompletedDate).HasColumnType("datetime");
            entity.Property(e => e.IsCompleted).HasDefaultValue(false);
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.AssignedToUser).WithMany(p => p.AssignedToTasks)
                .HasForeignKey(d => d.AssignedTo)
                .HasConstraintName("FK__Task__AssignedTo__3E52440B");

            entity.HasOne(d => d.AssignedByUser).WithMany(p => p.AssignedByTasks)
            .HasForeignKey(d => d.AssignedBy)
                .HasConstraintName("FK__Task__AssignedBy__3D5E1FD2");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Team__3214EC078AE51FEA");

            entity.ToTable("Team");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_User__3214EC072F132144");

            entity.ToTable("User");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EmailAddress).HasMaxLength(50);
            entity.Property(e => e.InactivatedDate).HasColumnType("datetime");
            entity.Property(e => e.Salt)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Team).WithMany(p => p.Users)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK__User__TeamId__286302EC");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserRole__3214EC075B88DB08");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__UserRoles__RoleI__2E1BDC42");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserRoles__UserI__2D27B809");
        });

        modelBuilder.Entity<RefreshTokens>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC072587126F");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.RefreshToken).HasMaxLength(50);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.RefreshTokenExpiry).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
