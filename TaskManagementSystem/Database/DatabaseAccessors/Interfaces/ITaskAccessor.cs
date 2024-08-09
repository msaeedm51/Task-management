using TaskManagementSystem.Database.Models;

namespace TaskManagementSystem.DatabaseAccessors.Interfaces
{
    public interface ITaskAccessor
    {
        Task<IEnumerable<Tasks>> GetAllAsync();
        Task<Tasks?> GetByIdAsync(int id);
        Task<IEnumerable<Tasks>> GetByUserAsync(int userId);
        Task<Tasks?> AddTaskAsync(Tasks task);
        Task<Tasks?> UpdateTaskAsync(Tasks task);
        Task<bool> DeleteTaskAsync(int id);
    }
}
