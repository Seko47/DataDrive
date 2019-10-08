using DataDrive.DAO.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.DAO.Models.Base
{
    public class FileAbstract : IBaseModel
    {
        public Guid ID { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public string Name { get; set; }
        public Guid ParentDirectoryID { get; set; }

        public virtual FileAbstract ParentDirectory { get; set; }
    }
}
