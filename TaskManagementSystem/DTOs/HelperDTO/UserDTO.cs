namespace TaskManagementSystem.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsManager { get; set; }
        public bool IsTeamLead { get; set; }
    }
}
