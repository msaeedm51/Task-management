using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DatabaseAccessors.Interfaces;
using TaskManagementSystem.DTOs;
using TaskManagementSystem.DTOs.RequestDTO;
using TaskManagementSystem.Service.Interfaces;
namespace TaskManagementSystem.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskAccessor _taskAccessor;
        private readonly IDocumentService _documentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TaskService(ITaskAccessor taskAccessor,
                           IDocumentService documentService,
                           IHttpContextAccessor httpContextAccessor)
        {
            _taskAccessor = taskAccessor;
            _documentService = documentService;   
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Tasks?> AddTask(RequestCreateTasksDTO task)
        {
            string? userId = _httpContextAccessor.HttpContext?.User?.Claims?
                                                     .FirstOrDefault(d => d.Type == "User_Id")?.Value ?? string.Empty;

            if (userId is null) return null;

            int id = Convert.ToInt32(userId);
            
            Tasks taskDb = new Tasks()
            {
                Description = task.Description,
                Title = task.Title,
                DueDate = task.DueDate,
                IsCompleted = false,
                AssignedTo = task.AssignedTo,
                AssignedBy = id
            };
            return await _taskAccessor.AddTaskAsync(taskDb);
        }

        public async Task<bool> DeleteTask(int id)
        {
            await _documentService.DeleteDocumentByTaskAsync(id);
            return await _taskAccessor.DeleteTaskAsync(id);
        }

        public async Task<IEnumerable<Tasks>> GetAll()
        {
            return await _taskAccessor.GetAllAsync();
        }

        public async Task<Tasks?> GetById(int id)
        {
            return await _taskAccessor.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Tasks>> GetByUser(int userId)
        {
            return await _taskAccessor.GetByUserAsync(userId);
        }

        public async Task<Tasks?> UpdateTask(RequestUpdateTaskDTO task)
        {
            Tasks? taskDb = await GetById(task.Id);
            if (taskDb == null) return null;

            taskDb.Description = task.Description;
            taskDb.Title = task.Title;
            taskDb.DueDate = task.DueDate;
            taskDb.IsCompleted = task.IsCompleted;
            taskDb.CompletedDate = task.CompletedDate;
            taskDb.UpdatedDate = DateTime.UtcNow;
            taskDb.AssignedTo = task.AssignedTo;
            return await _taskAccessor.UpdateTaskAsync(taskDb);
        }
    }
}
