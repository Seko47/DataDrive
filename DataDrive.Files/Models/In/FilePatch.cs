using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.Files.Models.In
{
    public class FilePatch
    {
        public string Name { get; set; }
        public Guid ParentDirectoryID { get; set; }
    }

    public class JsonPatchDocument_Mapper : Profile
    {
        public JsonPatchDocument_Mapper()
        {
            CreateMap(typeof(JsonPatchDocument<>), typeof(JsonPatchDocument<>));
            CreateMap(typeof(Operation<>), typeof(Operation<>));
        }
    }
}
