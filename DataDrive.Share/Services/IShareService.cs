using DataDrive.DAO.Helpers.Communication;
using DataDrive.Share.Models;
using DataDrive.Share.Models.In;
using DataDrive.Share.Models.Out;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataDrive.Share.Services
{
    public interface IShareService
    {
        Task<bool> IsSharedForUser(Guid resourceId, string username);
        Task<bool> IsSharedForEveryone(Guid resourceId);
        Task<bool> IsShared(Guid resourceId);


        Task<StatusCode<ShareEveryoneOut>> GetShareForEveryoneByFileIdAndUser(Guid resourceId, string username);
        Task<StatusCode<ShareEveryoneOut>> GetShareForEveryoneByToken(string token);
        Task<StatusCode<ShareEveryoneOut>> GetShareForEveryoneByTokenAndPassword(string token, string password);
        Task<StatusCode<ShareEveryoneOut>> ShareForEveryone(Guid resourceId, string username, string password, DateTime? expirationDateTime, int? downloadLimit);
        Task<bool> CancelSharingForEveryone(Guid resourceId, string username);

        Task<StatusCode<List<ShareForUserOut>>> GetShareForUserByResourceIdAndOwner(Guid resourceId, string ownerUsername);
        Task<StatusCode<List<ShareForUserOut>>> GetShareForUserByUserAndFilter(ShareFilter shareFilter, string username);
        Task<StatusCode<ShareForUserOut>> ShareForUser(ShareForUserIn shareForUserIn, string ownerUsername);
        Task<bool> CancelSharingForUser(Guid shareId, string ownerUsername);
        Task<bool> ReportResource(Guid resourceId);
    }
}
