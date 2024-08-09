using TaskManagementSystem.Database.Models;

namespace TaskManagementSystem.DatabaseAccessors.Interfaces
{
    public interface INoteAccessor
    {
        Task<Note?> GetByIdAsync(int noteId);
        Task<IEnumerable<Note?>> GetByTaskIdAsync(int taskId);
        Task<Note?> AddNoteAsync(Note note);
        Task<Note?> UpdateNoteAsync(Note note);
        Task<bool> DeleteNoteAsync(int noteId, int taskId);
        Task<bool> DeleteNoteByTaskAsync(int taskId);
    }
}
