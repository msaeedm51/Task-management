using TaskManagementSystem.Database.Models;

namespace TaskManagementSystem.DatabaseAccessors.Interfaces
{
    public interface IDocumentAccessor
    {
        Task<Document?> GetByIdAsync(int documentId);
        Task<IEnumerable<Document?>> GetByTaskIdAsync(int taskId);
        Task AddDocumentAsync(Document document);
        Task<bool> DeleteDocumentAsync(int documentId,int taskId);

        Task<bool> DeleteDocumentByTaskAsync(int taskId);
    }
}
