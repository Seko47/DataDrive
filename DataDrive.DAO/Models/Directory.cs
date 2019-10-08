using DataDrive.DAO.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.DAO.Models
{
    public class Directory : FileAbstract
    {
        public virtual List<FileAbstract> Files { get; set; }
    }
}
