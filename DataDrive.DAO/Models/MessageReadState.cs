using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.DAO.Models
{
    public class MessageReadState
    {
        public Guid ID { get; set; }
        public DateTime ReadDate { get; set; }

        public Guid MessageID { get; set; }
        public string UserID { get; set; }

        public virtual Message Message { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
