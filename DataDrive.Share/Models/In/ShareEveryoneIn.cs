using System;

namespace DataDrive.Share.Models.In
{
    public class ShareEveryoneIn
    {
        public Guid ResourceId { get; set; }
        public string Password { get; set; }
        public DateTime? ExpirationDateTime { get; set; }
        public int? DownloadLimit { get; set; }
    }
}
