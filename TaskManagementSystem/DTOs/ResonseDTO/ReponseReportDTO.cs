namespace TaskManagementSystem.DTOs
{
    public class ReponseReportDTO
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public int? TasksDueInWeek { get; set; }
        public int? TasksDueInMonth { get; set; }
        public int? TaskOverDue { get; set; }
        public int? TasksCompletedBeforeDueDate { get; set; }
        public int? TasksCompletedAfterDueDate { get; set; }
        public int? TasksCompletedOnDueDate { get; set; }
        public int? TotalTaskCompleted { get; set; }
        public int? TotalTaskPending { get; set; }
        public int? Totaltask { get; set; }

    }
}
