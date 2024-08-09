namespace TaskManagementSystem.DTOs
{
    public class ResponseUserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty; 
        public bool Active { get; set; }
        public List<string?> Roles { get; set; } = new List<string?>();
        public string Team { get; set; } = string.Empty;
    }
}
