using System;
using System.Collections.Generic;

namespace DataDrive.DAO.Models.Base
{
    public abstract class ResourceAbstract : IBaseModel
    {
        public Guid ID { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }

        public string Name { get; set; }

        public string OwnerID { get; set; }
        public Guid? ParentDirectoryID { get; set; }

        public ResourceType ResourceType { get; set; }

        public int NumberOfReports { get; set; }

        public bool IsShared { get; set; }
        public bool IsSharedForEveryone { get; set; }
        public bool IsSharedForUsers { get; set; }

        public virtual ApplicationUser Owner { get; set; }
        public virtual Directory ParentDirectory { get; set; }
        public virtual ShareEveryone ShareEveryone { get; set; }
        public virtual List<ShareForUser> ShareForUsers { get; set; }
    }

    public enum ResourceType
    {
        FILE,
        DIRECTORY,
        NOTE
    }
}
