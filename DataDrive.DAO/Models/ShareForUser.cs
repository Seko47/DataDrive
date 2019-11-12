using DataDrive.DAO.Models.Base;
using System;

namespace DataDrive.DAO.Models
{
    public class ShareForUser : ShareAbstract, ITimeExpiration
    {
        public string SharedForUserID { get; set; }

        public DateTime? ExpirationDateTime { get; set; }

        public virtual ApplicationUser SharedForUser { get; set; }
    }
}
