using AutoMapper;
using DataDrive.DAO.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.Messages.Models.Out
{
    public class MessageOut
    {
        public Guid ID { get; set; }
        public string Content { get; set; }
        public DateTime SentDate { get; set; }

        public string SendingUserID { get; set; }
        public string SendingUserUsername { get; set; }
        public Guid ThreadID { get; set; }

        public List<MessageReadStateOut> MessageReadStates { get; set; }
    }

    public class Message_to_MessageOut : Profile
    {
        public Message_to_MessageOut()
        {
            CreateMap<Message, MessageOut>()
                .ForMember(mo => mo.SendingUserUsername, opt => opt.MapFrom(m => m.SendingUser.UserName));
        }
    }
}
