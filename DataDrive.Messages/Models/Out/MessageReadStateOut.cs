using AutoMapper;
using DataDrive.DAO.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.Messages.Models.Out
{
    public class MessageReadStateOut
    {
        public Guid ID { get; set; }
        public DateTime ReadDate { get; set; }
        public Guid MessageID { get; set; }
        public string UserId { get; set; }
        public string UserUsername { get; set; }
    }

    public class MessageReadState_to_MessageReadStateOut : Profile
    {
        public MessageReadState_to_MessageReadStateOut()
        {
            CreateMap<MessageReadState, MessageReadStateOut>()
                .ForMember(mo => mo.UserUsername, opt => opt.MapFrom(m => m.User.UserName));
        }
    }
}
