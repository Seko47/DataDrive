using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.DAO.Models.Base
{
    public interface IBaseModel
    {
        public Guid ID { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
    }
}
