using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataDrive.DAO.Helpers.Communication;
using DataDrive.Notes.Models.In;
using DataDrive.Notes.Models.Out;
using Microsoft.AspNetCore.JsonPatch;

namespace DataDrive.Notes.Services
{
    public class NotesService : INotesService
    {
        public Task<StatusCode<Guid>> DeleteByIdAndUser(Guid noteId, string username)
        {
            throw new NotImplementedException();
        }

        public Task<StatusCode<List<NoteOut>>> GetAllByUser(string username)
        {
            throw new NotImplementedException();
        }

        public Task<StatusCode<NoteOut>> GetByIdAndUser(Guid noteId, string username)
        {
            throw new NotImplementedException();
        }

        public Task<StatusCode<NoteOut>> PatchByIdAndFilePatchAndUser(Guid guid, JsonPatchDocument<NotePatch> jsonPatchDocument, string v)
        {
            throw new NotImplementedException();
        }

        public Task<StatusCode<NoteOut>> PostNoteByUser(NotePost note, string username)
        {
            throw new NotImplementedException();
        }
    }
}
