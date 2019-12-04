using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.DAO.Models
{
    public class Message
    {
        public Guid ID { get; set; }
        public string Content { get; set; }
        public DateTime SentDate { get; set; }

        public string SendingUserID { get; set; }
        public Guid ThreadID { get; set; }

        public virtual ApplicationUser SendingUser { get; set; }
        public virtual MessageThread Thread { get; set; }
        public virtual List<MessageReadState> MessageReadStates { get; set; }
    }
}
