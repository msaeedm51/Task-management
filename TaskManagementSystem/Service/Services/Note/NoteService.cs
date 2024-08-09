using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DatabaseAccessors.Interfaces;
using TaskManagementSystem.DTOs;
using TaskManagementSystem.Service.Interfaces;

namespace TaskManagementSystem.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteAccessor _noteAccessor;
        public NoteService(INoteAccessor noteAccessor)
        {
            _noteAccessor = noteAccessor;
        }
        public async Task<Note?> AddNote(RequestCreateNoteDTO noteToCreate)
        {
            Note noteDb = new Note()
            {
                Content = noteToCreate.Note,
                TaskId = noteToCreate.TaskId,
                CreatedDate = DateTime.Now
            };

            return await _noteAccessor.AddNoteAsync(noteDb);
        }

        public async Task<bool> DeleteNote(int noteId, int taskId)
        {
            return await _noteAccessor.DeleteNoteAsync(noteId, taskId);
        }

        public async Task<bool> DeleteNoteByTask(int taskId)
        {
            return await _noteAccessor.DeleteNoteByTaskAsync(taskId);
        }

        public async Task<Note?> GetById(int noteId)
        {
            return await _noteAccessor.GetByIdAsync(noteId);
        }

        public async Task<IEnumerable<Note?>> GetByTaskId(int taskId)
        {
            return await _noteAccessor.GetByTaskIdAsync(taskId);
        }

        public async Task<Note?> UpdateNote(RequestUpdateNoteDTO noteToUpdate)
        {
            Note noteDb = new Note()
            {
                Id = noteToUpdate.NoteId,
                Content = noteToUpdate.Note
            };
            return await _noteAccessor.UpdateNoteAsync(noteDb);
        }
    }
}
