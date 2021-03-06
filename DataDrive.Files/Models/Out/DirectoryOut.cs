﻿using AutoMapper;
using DataDrive.DAO.Models;
using DataDrive.DAO.Models.Base;
using System.Collections.Generic;

namespace DataDrive.Files.Models.Out
{
    public class DirectoryOut : FileOut
    {
        public List<FileOut> Files { get; set; }
    }

    public class Directory_to_DirectoryOut : Profile
    {
        public Directory_to_DirectoryOut()
        {
            CreateMap<ResourceAbstract, FileOut>()
                .ForMember(fout => fout.ParentDirectoryName, opt => opt.MapFrom(f => f.ParentDirectory.Name));

            CreateMap<Directory, DirectoryOut>()
                .ForMember(fout => fout.ParentDirectoryName, opt => opt.MapFrom(f => f.ParentDirectory.Name));
        }
    }
}
