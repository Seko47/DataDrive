using AutoMapper;
using DataDrive.DAO.Models;
using DataDrive.DAO.Models.Base;
using System;

namespace DataDrive.Share.Models
{
    public class ShareEveryoneOut
    {
        public Guid ID { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }

        public string Token { get; set; }
        public DateTime? ExpirationDateTime { get; set; }
        public int? DownloadLimit { get; set; }

        public Guid ResourceID { get; set; }
        public string ResourceName { get; set; }
        public ResourceType ResourceType { get; set; }
        public string OwnerUsername { get; set; }
    }

    public class ShareEveryone_to_ShareEveryoneOut : Profile
    {
        public ShareEveryone_to_ShareEveryoneOut()
        {
            CreateMap<ShareEveryone, ShareEveryoneOut>()
                .ForMember(fout => fout.ResourceName, opt => opt.MapFrom(f => f.Resource.Name))
                .ForMember(fout => fout.OwnerUsername, opt => opt.MapFrom(f => f.Owner.UserName))
                .ForMember(fout => fout.ResourceType, opt => opt.MapFrom(f => f.Resource.ResourceType));
        }
    }
}
