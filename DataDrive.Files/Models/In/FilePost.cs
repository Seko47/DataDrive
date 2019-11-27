using Microsoft.AspNetCore.Http;
using System;

namespace DataDrive.Files.Models.In
{
    public class FilePost
    {
        public Guid? ParentDirectoryID { get; set; }
        public IFormFileCollection Files { get; set; }
    }
}
