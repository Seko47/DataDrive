using AutoMapper;
using DataDrive.DAO.Models;
using DataDrive.DAO.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

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
        public string Password { get; set; }

        public Guid FileID { get; set; }
        public string FileName { get; set; }
        public string OwnerUsername { get; set; }
    }

    public class ShareEveryone_to_ShareEveryoneOut : Profile
    {
        public ShareEveryone_to_ShareEveryoneOut()
        {
            CreateMap<ShareEveryone, ShareEveryoneOut>()
                .ForMember(fout => fout.FileName, opt => opt.MapFrom(f => f.File.Name))
                .ForMember(fout => fout.OwnerUsername, opt => opt.MapFrom(f => f.Owner.UserName));
        }
    }
}
