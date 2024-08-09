using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DTOs;

namespace TaskManagementSystem.MappingExtensionMethods
{
    public static class TaskMappingExtension
    {
        public static ResponseTaskDTO MapToTaskDTO(this Tasks task)
        {
            var responseTaskDTO = new ResponseTaskDTO()
            {
                TaskId = task.Id,
                Title = task.Title,
                Description = task.Description!,
                DueDate = task.DueDate,
                Status = task.IsCompleted == true ? "Completed" : "Pending",
                UserName = task.User!.Username
            };

            return responseTaskDTO;
        }
    }
}
