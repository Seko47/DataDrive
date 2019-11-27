using DataDrive.DAO.Helpers.Communication;
using DataDrive.Notes.Models.Out;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataDrive.Notes.Services
{
    public interface INotesService
    {
        Task<StatusCode<NoteOut>> GetByIdAndUser(Guid noteId, string username);
        Task<StatusCode<List<NoteOut>>> GetAllByUser(string username);
    }
}
