using AutoMapper;
using DataDrive.DAO.Context;
using DataDrive.DAO.Helpers.Communication;
using DataDrive.DAO.Models;
using DataDrive.Notes.Models.In;
using DataDrive.Notes.Models.Out;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataDrive.Notes.Services
{
    public class NotesService : INotesService
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public NotesService(IDatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }

        public async Task<StatusCode<Guid>> DeleteByIdAndUser(Guid noteId, string username)
        {
            string userId = (await _databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == username)).Id;

            Note noteToDelete = await _databaseContext.Notes.FirstOrDefaultAsync(_ => _.ID == noteId && _.OwnerID == userId);

            if (noteToDelete == null)
            {
                return new StatusCode<Guid>(StatusCodes.Status404NotFound, $"Note {noteId} not found");
            }

            _databaseContext.Notes.Remove(noteToDelete);
            await _databaseContext.SaveChangesAsync();

            if (_databaseContext.Notes.AnyAsync(_ => _.ID == noteId).Result)
            {
                return new StatusCode<Guid>(StatusCodes.Status400BadRequest, $"Note {noteId} cannot be deleted");
            }

            return new StatusCode<Guid>(StatusCodes.Status200OK, noteId);
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
