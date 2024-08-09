using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DTOs;

namespace TaskManagementSystem.MappingExtensionMethods
{
    public static class TeamMappingExtension
    {
        public static ResponseTeamDTO MapToTeamDTO(this Team team) 
        {
            ResponseTeamDTO responseTeamDTO = new()
            {
                TeamId = team.Id,
                TeamName = team.Name
            };

            return responseTeamDTO;
        }

        public static ReponseReportDTO MapToReportDTO(this Team team)
        {
            ReponseReportDTO reponseReportDTO = new()
            {
                TeamId = team.Id,
                TasksDueInWeek = team.Users.SelectMany(e => e.AssignedToTasks)
                    .Count(t => t.DueDate! <= DateTime.Now.AddDays(7) && t.IsCompleted == false),
                TasksDueInMonth = team.Users.SelectMany(e => e.AssignedToTasks)
                    .Count(t => t.DueDate! <= DateTime.Now.AddMonths(1) && t.IsCompleted == false),
                TaskOverDue = team.Users.SelectMany(e => e.AssignedToTasks)
                    .Count(t => t.DueDate! > DateTime.Now && t.IsCompleted == false),
                Totaltask = team.Users.SelectMany(e => e.AssignedToTasks).Count(),
                TotalTaskCompleted = team.Users.SelectMany(e => e.AssignedToTasks)
                    .Count(t => t.IsCompleted == true),
                TotalTaskPending = team.Users.SelectMany(e => e.AssignedToTasks)
                    .Count(t => t.IsCompleted == false),
                TasksCompletedAfterDueDate = team.Users.SelectMany(e => e.AssignedToTasks)
                    .Count(t => t.CompletedDate != null && t.CompletedDate! > t.DueDate! && t.IsCompleted == true),
                TasksCompletedBeforeDueDate = team.Users.SelectMany(e => e.AssignedToTasks)
                    .Count(t => t.CompletedDate != null && t.CompletedDate! < t.DueDate! && t.IsCompleted == true),
                 TasksCompletedOnDueDate = team.Users.SelectMany(e => e.AssignedToTasks)
                    .Count(t => t.CompletedDate != null && t.CompletedDate! == t.DueDate! && t.IsCompleted == true)

            };

            return reponseReportDTO;
        }
    }
}
