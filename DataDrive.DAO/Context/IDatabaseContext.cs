﻿using DataDrive.DAO.Models;
using DataDrive.DAO.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace DataDrive.DAO.Context
{
    public interface IDatabaseContext
    {
        DbSet<FileAbstract> FileAbstracts { get; set; }
        DbSet<Directory> Directories { get; set; }
        DbSet<File> Files { get; set; }
        DbSet<Note> Notes { get; set; }
    }
}