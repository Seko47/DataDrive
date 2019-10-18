using System;

namespace DataDrive.Files.Models.In
{
    public class DirectoryPost
    {
        public Guid? ParentDirectoryID { get; set; }
        public string Name { get; set; }
    }
}
