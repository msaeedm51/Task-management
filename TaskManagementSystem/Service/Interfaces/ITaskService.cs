using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DTOs;
using TaskManagementSystem.DTOs.RequestDTO;

namespace TaskManagementSystem.Service.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<Tasks>> GetAll();
        Task<Tasks?> GetById(int id);
        Task<IEnumerable<Tasks>> GetByUser(int userId);
        Task<Tasks?> AddTask(RequestCreateTasksDTO task);
        Task<Tasks?> UpdateTask(RequestUpdateTaskDTO task);
        Task<bool> DeleteTask(int id);
    }
}
