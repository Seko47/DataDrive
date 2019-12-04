using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.DAO.Models
{
    public class MessageThread
    {
        public Guid ID { get; set; }

        public virtual List<Message> Messages { get; set; }
        public virtual List<MessageThreadParticipant> MessageThreadParticipants { get; set; }
    }
}
