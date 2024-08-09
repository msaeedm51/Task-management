using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Database;
using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DatabaseAccessors.Interfaces;

namespace TaskManagementSystem.DatabaseAccessors
{
    public class DocumentAccessor : IDocumentAccessor
    {
        private readonly TaskManagementDbContext _context;
        public DocumentAccessor(TaskManagementDbContext context) 
        { 
            _context = context;
        }

        public async Task AddDocumentAsync(Document document)
        {
            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteDocumentAsync(int documentId, int taskId)
        {
            Document? document = await GetByIdAsync(documentId);
            if(document is null) return false;

            if(document.TaskId != taskId) return false;

            _context.Documents.Remove(document);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteDocumentByTaskAsync(int taskId)
        {
            await _context.Documents.Where(d=> d.TaskId == taskId).ExecuteDeleteAsync();
            return true;
        }

        public async Task<Document?> GetByIdAsync(int documentId)
        {
            return await _context.Documents.FirstOrDefaultAsync(d=> d.Id == documentId);
        }

        public async Task<IEnumerable<Document?>> GetByTaskIdAsync(int taskId)
        {
            return await _context.Documents.Where(d => d.TaskId == taskId).ToListAsync();
        }
    }
}
