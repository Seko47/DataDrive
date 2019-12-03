using System;

namespace DataDrive.DAO.Models.Base
{
    public abstract class ShareAbstract : IBaseModel
    {
        public Guid ID { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }

        public Guid ResourceID { get; set; }
        public string OwnerID { get; set; }

        public virtual FileAbstract Resource { get; set; }
        public virtual ApplicationUser Owner { get; set; }
    }
}
