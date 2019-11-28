using AutoMapper;
using DataDrive.DAO.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace DataDrive.Notes.Models.In
{
    public class NotePatch
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class JsonPatchDocument_NotePatch_Mapper : Profile
    {
        public JsonPatchDocument_NotePatch_Mapper()
        {
            CreateMap(typeof(JsonPatchDocument<NotePatch>), typeof(JsonPatchDocument<Note>));
            CreateMap(typeof(Operation<NotePatch>), typeof(Operation<Note>));
        }
    }
}
