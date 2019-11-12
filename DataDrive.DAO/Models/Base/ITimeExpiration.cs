using System;

namespace DataDrive.DAO.Models.Base
{
    public interface ITimeExpiration
    {
        public DateTime? ExpirationDateTime { get; set; }
    }
}
