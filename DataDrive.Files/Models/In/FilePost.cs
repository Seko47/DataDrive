using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace DataDrive.Files.Models.In
{
    public class FilePost
    {
        public Guid? ParentDirectoryID { get; set; }
        public IFormFileCollection Files { get; set; }
    }
}
