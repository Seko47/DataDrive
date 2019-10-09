using System;

namespace DataDrive.DAO.Models.Base
{
    public class FileAbstract : IBaseModel
    {
        public Guid ID { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public Guid OwnerID { get; set; }
        public Guid? ParentDirectoryID { get; set; }

        public virtual ApplicationUser Owner { get; set; }
        public virtual Directory ParentDirectory { get; set; }
    }
}
