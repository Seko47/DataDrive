using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.DAO.Models.Base
{
    public interface IDownloadLimitCounter
    {
        public int DownloadLimit { get; set; }
    }
}
