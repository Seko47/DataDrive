using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataDrive.DAO.Helpers.Communication;
using DataDrive.Notes.Models.Out;

namespace DataDrive.Notes.Services
{
    public class NotesService : INotesService
    {
        public Task<StatusCode<List<NoteOut>>> GetAllByUser(string username)
        {
            throw new NotImplementedException();
        }

        public Task<StatusCode<NoteOut>> GetByIdAndUser(Guid noteId, string username)
        {
            throw new NotImplementedException();
        }
    }
}
