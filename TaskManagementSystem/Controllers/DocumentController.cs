using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Service.Interfaces;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        public DocumentController(IDocumentService documentService) 
        {
          _documentService = documentService;
        }

        [HttpGet("download/{documentId}")]
        public async Task<IActionResult> DownloadDocument(int documentId)
        {
            try
            {
                if (documentId == 0) return BadRequest();

                var document = await _documentService.GetById(documentId);
                if (document is null) return BadRequest();

                var fileByte = System.IO.File.ReadAllBytes(document.FilePath!);
                return File(fileByte, "application/octet-stream", document.FileName);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while Downloading the document : {0} : {1}", ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }

        [HttpDelete("{documentId}/task/{taskid}")]
        public async Task<IActionResult> DeleteDocument(int documentId, int taskid)
        {
            try
            {
                if (documentId == 0) return BadRequest();

                await _documentService.DeleteDocumentAsync(documentId, taskid);

                return NoContent();
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while Deleting document from the database : {0} : {1}", ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }
    }
}
