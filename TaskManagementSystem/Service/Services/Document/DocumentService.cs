using Microsoft.Extensions.Options;
using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DatabaseAccessors.Interfaces;
using TaskManagementSystem.DTOs;
using TaskManagementSystem.Service.Interfaces;

namespace TaskManagementSystem.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentAccessor _documentAccessor;
        private readonly UploadPath _uploadPath;
        public DocumentService(IDocumentAccessor documentAccessor,
                              IOptions<UploadPath> options)
        {
            _documentAccessor = documentAccessor;
            _uploadPath = options.Value;
        }


        public async Task AddDocumentAsync(int taskId, IFormFile document)
        {
            if (document is not null)
            {
                string randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(document.FileName)}";
                Document documentDB = new()
                {
                    TaskId = taskId,
                    FileName = document.FileName,
                    FilePath = Path.Combine(Directory.GetCurrentDirectory(), _uploadPath.Path, randomFileName)
                };
                await _documentAccessor.AddDocumentAsync(documentDB);

                await document.CopyToAsync(new FileStream(documentDB.FilePath, FileMode.Create));
            }
        }

        public async Task<bool> DeleteDocumentAsync(int documentId, int taskId)
        {
            Document? document = await GetById(documentId);
            if (document is not null) return false;
            if(document?.TaskId != taskId) return false;
            await _documentAccessor.DeleteDocumentAsync(documentId, taskId);
            File.Delete(document.FilePath!);

            return true;
        }

        public async Task<bool> DeleteDocumentByTaskAsync(int taskId)
        {
            var documents = await _documentAccessor.GetByTaskIdAsync(taskId);
            await _documentAccessor.DeleteDocumentByTaskAsync(taskId);
            foreach (var document in documents)
            {
                File.Delete(document?.FilePath!);
            }
            return true;
        }

        public async Task<Document?> GetById(int documentId)
        {
            return await _documentAccessor.GetByIdAsync(documentId);
        }
    }
}
