using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.DAO.Models
{
    public class MessageThreadParticipant
    {
        public Guid ID { get; set; }
        public Guid ThreadID { get; set; }
        public string UserID { get; set; }

        public virtual MessageThread Thread { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
