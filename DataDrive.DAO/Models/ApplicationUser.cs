﻿using DataDrive.DAO.Models.Base;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DataDrive.DAO.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ulong TotalDiskSpace { get; set; }
        public ulong UsedDiskSpace { get; set; }
        public virtual List<ResourceAbstract> Files { get; set; }
        public virtual List<ShareAbstract> SharedOwn { get; set; }
        public virtual List<ShareForUser> SharedForUser { get; set; }

        public ulong FreeDiskSpace
        {
            get => TotalDiskSpace - UsedDiskSpace;
        }
    }
}
