using AutoMapper;
using DataDrive.DAO.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.Messages.Models.Out
{
    public class MessageThreadParticipantOut
    {
        public Guid ID { get; set; }
        public Guid ThreadID { get; set; }
        public string UserID { get; set; }
        public string UserUsername { get; set; }
    }

    public class MessageThreadParticipant_to_MessageThreadParticipantOut : Profile
    {
        public MessageThreadParticipant_to_MessageThreadParticipantOut()
        {
            CreateMap<MessageThreadParticipant, MessageThreadParticipantOut>()
                .ForMember(mo => mo.UserUsername, opt => opt.MapFrom(m => m.User.UserName));
        }
    }
}
