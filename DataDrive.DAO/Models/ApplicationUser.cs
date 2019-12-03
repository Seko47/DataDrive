using DataDrive.DAO.Models.Base;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DataDrive.DAO.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual List<ResourceAbstract> Files { get; set; }
        public virtual List<ShareAbstract> SharedOwn { get; set; }
        public virtual List<ShareForUser> SharedForUser { get; set; }
    }
}
