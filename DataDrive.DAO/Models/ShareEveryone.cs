using DataDrive.DAO.Models.Base;
using System;

namespace DataDrive.DAO.Models
{
    public class ShareEveryone : ShareAbstract, ITimeExpiration, IDownloadLimitCounter, IPasswordDownload
    {
        public string Token { get; set; }

        public DateTime? ExpirationDateTime { get; set; }
        public int? DownloadLimit { get; set; }
        public string Password { get; set; }
    }
}
