using DataDrive.DAO.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.DAO.Models
{
    public class Note : FileAbstract
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
