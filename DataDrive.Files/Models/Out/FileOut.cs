﻿using AutoMapper;
using DataDrive.DAO.Models.Base;
using System;

namespace DataDrive.Files.Models.Out
{
    public class FileOut
    {
        public Guid? ID { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }

        public Guid? ParentDirectoryID { get; set; }
        public string ParentDirectoryName { get; set; }

        public string Name { get; set; }
        public ResourceType ResourceType { get; set; }

        public ulong FileSizeBytes { get; set; }

        public bool IsShared { get; set; }
        public bool IsSharedForEveryone { get; set; }
        public bool IsSharedForUsers { get; set; }
    }

    public class FileAbstract_to_FileOut : Profile
    {
        public FileAbstract_to_FileOut()
        {
            CreateMap<ResourceAbstract, FileOut>()
                .ForMember(fout => fout.ParentDirectoryName, opt => opt.MapFrom(f => f.ParentDirectory.Name));
        }
    }
}
