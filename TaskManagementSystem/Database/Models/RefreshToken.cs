namespace TaskManagementSystem.Database.Models
{
    public class RefreshTokens
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string RefreshToken { get; set; } = null!;

        public bool? Active { get; set; }

        public DateTime RefreshTokenExpiry { get; set; }

    }
}
