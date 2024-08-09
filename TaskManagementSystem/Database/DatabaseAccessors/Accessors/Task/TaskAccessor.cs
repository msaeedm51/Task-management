using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Database;
using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DatabaseAccessors.Interfaces;

namespace TaskManagementSystem.DatabaseAccessors
{
    public class TaskAccessor : ITaskAccessor
    {
        private readonly TaskManagementDbContext _context;
        public TaskAccessor(TaskManagementDbContext context)
        {
            _context = context;
        }
        public async Task<Tasks?> AddTaskAsync(Tasks task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(task.Id); 
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var taskDb = await GetByIdAsync(id);
            if (taskDb is null) return false;

            _context.Tasks.Remove(taskDb);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Tasks>> GetAllAsync()
        {
            return await _context.Tasks
                .Include(t => t.AssignedToUser)
                .Include(t => t.AssignedByUser)
                .ToListAsync();
        }

        public async Task<Tasks?> GetByIdAsync(int id)
        {
            return await _context.Tasks
                        .Include(t => t.Documents)
                        .Include(t => t.AssignedToUser)
                        .Include(t => t.AssignedByUser)
                        .Include(t => t.Notes)
                        .FirstOrDefaultAsync(d=> d.Id == id);
        }

        public async Task<IEnumerable<Tasks>> GetByUserAsync(int userId)
        {
            return await _context.Tasks
                         .Include(t => t.AssignedToUser)
                         .Include(t => t.AssignedByUser)
                         .Where(d => d.AssignedTo == userId)
                        .ToListAsync();
        }

        public async Task<Tasks?> UpdateTaskAsync(Tasks task)
        {
            var taskDb = await GetByIdAsync(task.Id);
            if (taskDb is null) return null;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(task.Id);
        }
    }
}
