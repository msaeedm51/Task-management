using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Database;
using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DatabaseAccessors.Interfaces;

namespace TaskManagementSystem.DatabaseAccessors
{
    public class NoteAccessor : INoteAccessor
    {
        private readonly TaskManagementDbContext _context;
        public NoteAccessor(TaskManagementDbContext context)
        {
            _context = context;
        }
        public async Task<Note?> AddNoteAsync(Note note)
        {
            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(note.Id);
        }

        public async Task<bool> DeleteNoteAsync(int noteId, int taskId)
        {
            var noteDB = await GetByIdAsync(noteId);
            if (noteDB is null || noteDB.TaskId != taskId) return false;

            _context.Notes.Remove(noteDB);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteNoteByTaskAsync(int taskId)
        {
            await _context.Notes.Where(d => d.TaskId == taskId).ExecuteDeleteAsync();
            return true;
        }

        public async Task<Note?> GetByIdAsync(int noteId)
        {
            return await _context.Notes.FirstOrDefaultAsync(d => d.Id == noteId);
        }

        public async Task<IEnumerable<Note?>> GetByTaskIdAsync(int taskId)
        {
            return await _context.Notes.Where(d=> d.TaskId == taskId).ToListAsync();
        }

        public async Task<Note?> UpdateNoteAsync(Note note)
        {
            var noteDB = await GetByIdAsync(note.Id);
            if (noteDB is null) return null;


             noteDB.Content = note.Content;
             noteDB.UpdatedDate = DateTime.UtcNow;
            _context.Notes.Update(note);
            await _context.SaveChangesAsync();
            return noteDB;

        }
    }
}
