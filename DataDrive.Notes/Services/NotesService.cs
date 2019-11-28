using AutoMapper;
using DataDrive.DAO.Context;
using DataDrive.DAO.Helpers.Communication;
using DataDrive.DAO.Models;
using DataDrive.Notes.Models.In;
using DataDrive.Notes.Models.Out;
using DataDrive.Share.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            string userId = (await _databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == username))?
                .Id;

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

        public async Task<StatusCode<List<NoteOut>>> GetAllByUser(string username)
        {
            string userId = (await _databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == username))?
                .Id;

            List<NoteOut> notes = _mapper.Map<List<NoteOut>>(await _databaseContext.Notes
                .Where(_ => _.OwnerID == userId)
                .ToListAsync());

            if (notes == null || !notes.Any())
            {
                return new StatusCode<List<NoteOut>>(StatusCodes.Status404NotFound, $"Notes not found");
            }

            return new StatusCode<List<NoteOut>>(StatusCodes.Status200OK, notes);
        }

        public async Task<StatusCode<NoteOut>> GetByIdAndUser(Guid noteId, string username)
        {
            string userId = (await _databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == username))?
                .Id;

            Note note = await _databaseContext.Notes
                .Include(_ => _.ShareEveryone)
                .Include(_ => _.ShareForUsers)
                .FirstOrDefaultAsync(_ => _.ID == noteId);

            if (note == null)
            {
                return new StatusCode<NoteOut>(StatusCodes.Status404NotFound, $"Note {noteId} not found");
            }

            if (note.OwnerID == userId)
            {
                return new StatusCode<NoteOut>(StatusCodes.Status200OK, _mapper.Map<NoteOut>(note));
            }

            if (note.IsShared)
            {
                if (note.IsSharedForEveryone)
                {
                    ShareEveryone share = note.ShareEveryone;

                    if (share != null)
                    {
                        if ((share.ExpirationDateTime == null
                                || (share.ExpirationDateTime != null && share.ExpirationDateTime >= DateTime.Now))
                           && (share.DownloadLimit == null
                                || (share.DownloadLimit != null && share.DownloadLimit > 0)))
                        {
                            return new StatusCode<NoteOut>(StatusCodes.Status200OK, _mapper.Map<NoteOut>(note));
                        }
                    }
                }
                else if (note.IsSharedForUsers)
                {
                    ShareForUser share = note.ShareForUsers.FirstOrDefault(_ => _.SharedForUserID == userId);

                    if (share != null)
                    {
                        if (share.ExpirationDateTime == null
                            || (share.ExpirationDateTime != null && share.ExpirationDateTime >= DateTime.Now))
                        {
                            return new StatusCode<NoteOut>(StatusCodes.Status200OK, _mapper.Map<NoteOut>(note));
                        }
                    }
                }
            }

            return new StatusCode<NoteOut>(StatusCodes.Status404NotFound, $"Note {noteId} not found");
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
