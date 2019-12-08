using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.Share.Models.In
{
    public class ShareForUserIn
    {
        public Guid ResourceId { get; set; }

        public DateTime? ExpirationDateTime { get; set; }
        public string Username { get; set; }
    }
}
