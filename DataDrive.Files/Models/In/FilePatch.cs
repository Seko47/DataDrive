﻿using AutoMapper;
using DataDrive.DAO.Models.Base;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;

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
            CreateMap(typeof(JsonPatchDocument<FilePatch>), typeof(JsonPatchDocument<ResourceAbstract>));
            CreateMap(typeof(Operation<FilePatch>), typeof(Operation<ResourceAbstract>));
        }
    }
}
