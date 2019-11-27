using AutoMapper;
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
        public FileType FileType { get; set; }
    }

    public class FileAbstract_to_NoteOut : Profile
    {
        public FileAbstract_to_NoteOut()
        {
            CreateMap<FileAbstract, NoteOut>();
        }
    }
}
