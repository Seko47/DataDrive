using AutoMapper;
using DataDrive.DAO.Models.Base;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace DataDrive.Notes.Models.In
{
    public class NotePatch
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class JsonPatchDocument_Mapper : Profile
    {
        public JsonPatchDocument_Mapper()
        {
            CreateMap(typeof(JsonPatchDocument<NotePatch>), typeof(JsonPatchDocument<FileAbstract>));
            CreateMap(typeof(Operation<NotePatch>), typeof(Operation<FileAbstract>));
        }
    }
}
