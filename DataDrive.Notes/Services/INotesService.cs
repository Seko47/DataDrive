using DataDrive.DAO.Helpers.Communication;
using DataDrive.Notes.Models.In;
using DataDrive.Notes.Models.Out;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataDrive.Notes.Services
{
    public interface INotesService
    {
        Task<StatusCode<NoteOut>> GetByIdAndUser(Guid noteId, string username);
        Task<StatusCode<List<NoteOut>>> GetAllByUser(string username);
        Task<StatusCode<Guid>> DeleteByIdAndUser(Guid noteId, string username);
        Task<StatusCode<NoteOut>> PatchByIdAndNotePatchAndUser(Guid noteId, JsonPatchDocument<NotePatch> jsonPatchDocument, string username);
        Task<StatusCode<NoteOut>> PostNoteByUser(NotePost note, string username);
    }
}
