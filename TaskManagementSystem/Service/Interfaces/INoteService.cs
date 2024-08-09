using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DTOs;

namespace TaskManagementSystem.Service.Interfaces
{
    public interface INoteService
    {
        Task<Note?> GetById(int noteId);
        Task<IEnumerable<Note?>> GetByTaskId(int taskId);
        Task<Note?> AddNote(RequestCreateNoteDTO noteToCreate);
        Task<Note?> UpdateNote(RequestUpdateNoteDTO noteToUpdate);
        Task<bool> DeleteNote(int noteId, int taskId);
        Task<bool> DeleteNoteByTask(int taskId);
    }
}
