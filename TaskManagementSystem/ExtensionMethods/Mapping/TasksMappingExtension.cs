using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DTOs;

namespace TaskManagementSystem.MappingExtensionMethods
{
    public static class TasksMappingExtension
    {
        public static ResponseTaskDTO MapToTaskDTO(this Tasks task)
        {
            ResponseTaskDTO responseTaskDTO = new()
            {
                TaskId = task.Id,
                Title = task.Title,
                Description = task.Description!,
                DueDate = task.DueDate,
                Status = task.IsCompleted == true ? "Completed" : "Pending",
                AssignedToUser = task.AssignedToUser!.Username,
                AssignedByUser = task.AssignedByUser!.Username,
                Documents = task.Documents?.Select(d => d.FileName!).ToList() ?? new List<string>()
            };

            return responseTaskDTO;
        }
    }
}
