using DataDrive.DAO.Models.Base;
using System.Collections.Generic;

namespace DataDrive.DAO.Models
{
    public class Directory : ResourceAbstract
    {
        public virtual List<ResourceAbstract> Files { get; set; }
    }
}
