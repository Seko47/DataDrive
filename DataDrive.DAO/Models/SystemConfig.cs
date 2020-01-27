using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.DAO.Models
{
    public class SystemConfig
    {
        public Guid ID { get; set; }
        public ulong TotalDiskSpaceForNewUser { get; set; }
    }
}
