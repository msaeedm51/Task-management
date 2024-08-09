using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DTOs;

namespace TaskManagementSystem.MappingExtensionMethods
{
    public static class NoteMappingExtension
    {
        public static ResponseNoteDTO MapToNoteDTO(this Note note)
        {
            ResponseNoteDTO responseNoteDTO = new()
            {
                NoteId = note.Id,
                TaskId = note.TaskId ?? 0,
                Content = note.Content,
                CreatedDate = note.CreatedDate
            };
            return responseNoteDTO;
        }
    }
}
