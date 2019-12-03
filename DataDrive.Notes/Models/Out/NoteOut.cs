using AutoMapper;
using DataDrive.DAO.Models;
using DataDrive.DAO.Models.Base;
using System;

namespace DataDrive.Notes.Models.Out
{
    public class NoteOut
    {
        public Guid ID { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }

        public bool IsShared { get; set; }
        public bool IsSharedForEveryone { get; set; }
        public bool IsSharedForUsers { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public ResourceType ResourceType { get; set; }
    }

    public class Note_to_NoteOut : Profile
    {
        public Note_to_NoteOut()
        {
            CreateMap<Note, NoteOut>();
        }
    }
}
