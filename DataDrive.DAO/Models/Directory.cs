using DataDrive.DAO.Models.Base;
using System.Collections.Generic;

namespace DataDrive.DAO.Models
{
    public class Directory : FileAbstract
    {
        public string Name { get; set; }

        public virtual List<FileAbstract> Files { get; set; }
    }
}
