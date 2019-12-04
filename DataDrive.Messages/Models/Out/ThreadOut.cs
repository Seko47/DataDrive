using AutoMapper;
using DataDrive.DAO.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.Messages.Models.Out
{
    public class ThreadOut
    {
        public Guid ID { get; set; }

        public List<MessageOut> Messages { get; set; }
        public List<MessageThreadParticipantOut> MessageThreadParticipants { get; set; }

    }

    public class MessageThread_to_ThreadOut : Profile
    {
        public MessageThread_to_ThreadOut()
        {
            CreateMap<MessageThread, ThreadOut>();
        }
    }
}
