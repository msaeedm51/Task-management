using TaskManagementSystem.Database.Models;

namespace TaskManagementSystem.Service.Interfaces
{
    public interface IDocumentService
    {
        Task<Document?> GetById(int documentId);
        Task AddDocumentAsync(int taskId, IFormFile document);
        Task<bool> DeleteDocumentAsync(int documentId, int taskId);

        Task<bool> DeleteDocumentByTaskAsync(int taskId);
    }
}
