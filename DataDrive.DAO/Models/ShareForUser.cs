using DataDrive.DAO.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.DAO.Models
{
    public class ShareForUser : ShareAbstract, ITimeExpiration
    {
        public Guid SharedForUserID { get; set; }

        public DateTime ExpirationDateTime { get; set; }

        public virtual ApplicationUser SharedForUser { get; set; }
    }
}
