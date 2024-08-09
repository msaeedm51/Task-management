using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DTOs;
using TaskManagementSystem.MappingExtensionMethods;
using TaskManagementSystem.Service.Interfaces;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet("{noteId}")]
        public async Task<IActionResult> GetNoteById(int noteId)
        {
            try
            {
                var note = await _noteService.GetById(noteId);
                if (note is null)
                {
                    return NotFound("No Note found");
                }
                return Ok(note.MapToNoteDTO());
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while getting a Note with id:{0} from the database : {1} : {2}", noteId, ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }

        [HttpGet("{taskId}/task")]
        public async Task<IActionResult> GetNoteBytaskId(int taskId)
        {
            try
            {
                var notes = await _noteService.GetByTaskId(taskId);
                if (notes is null)
                {
                    return NotFound("No notes found");
                }
                return Ok(notes.Select(t => t!.MapToNoteDTO()));
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while getting a note with task id:{0} from the database : {1} : {2}", taskId, ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] RequestCreateNoteDTO noteToCreate)
        {
            try
            {
                if (noteToCreate is null)
                {
                    return BadRequest();
                }

                Note? noteDB = await _noteService.AddNote(noteToCreate);
                if (noteDB is null) return BadRequest();

                return CreatedAtAction(nameof(CreateNote), new { id = noteDB!.Id }, noteDB!.MapToNoteDTO());
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while creating note from the database : {0} : {1}", ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }

        [HttpPut("{noteId}")]
        public async Task<IActionResult> UpdateNote(int noteId, [FromBody] RequestUpdateNoteDTO noteToUpdate)
        {
            try
            {
                if (noteId != noteToUpdate.NoteId)
                {
                    return BadRequest();
                }

                var updatedNote = await _noteService.UpdateNote(noteToUpdate);
                if (updatedNote == null) return NotFound("No such note found");


                return NoContent();
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while Updating note from the database : {0} : {1}", ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }

        [HttpDelete("{noteId}/task/{taskid}")]
        public async Task<IActionResult> DeleteNote(int noteId, int taskid)
        {
            try
            {
                var success = await _noteService.DeleteNote(noteId, taskid);
                if (success == false) return NotFound("No Such note found");

                return NoContent();
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while Deleting note from the database : {0} : {1}", ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }
    }
}
