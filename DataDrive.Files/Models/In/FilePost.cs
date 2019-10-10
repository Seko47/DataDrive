using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.Files.Models.In
{
    public class FilePost
    {
        public Guid? ParentDirectoryID { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
