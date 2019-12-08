using AutoMapper;
using DataDrive.DAO.Models;
using DataDrive.DAO.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.Share.Models.Out
{
    public class ShareForUserOut
    {
        public Guid ID { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }

        public string SharedForUserID { get; set; }
        public string SharedForUserUsername { get; set; }

        public DateTime? ExpirationDateTime { get; set; }

        public Guid ResourceID { get; set; }
        public string ResourceName { get; set; }
        public ResourceType ResourceType { get; set; }
        public string OwnerUsername { get; set; }
    }

    public class ShareForUser_to_ShareForUserOut : Profile
    {
        public ShareForUser_to_ShareForUserOut()
        {
            CreateMap<ShareForUser, ShareForUserOut>()
                .ForMember(so => so.SharedForUserUsername, opt => opt.MapFrom(s => s.SharedForUser.UserName))
                .ForMember(so => so.ResourceName, opt => opt.MapFrom(s => s.Resource.Name))
                .ForMember(so => so.OwnerUsername, opt => opt.MapFrom(s => s.Owner.UserName));
        }
    }
}
