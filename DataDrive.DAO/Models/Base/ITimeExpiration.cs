using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.DAO.Models.Base
{
    public interface ITimeExpiration
    {
        public DateTime ExpirationDateTime { get; set; }
    }
}
