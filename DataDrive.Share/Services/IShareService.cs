using DataDrive.DAO.Helpers.Communication;
using DataDrive.Share.Models;
using System;
using System.Threading.Tasks;

namespace DataDrive.Share.Services
{
    public interface IShareService
    {
        Task<bool> IsSharedForUser(Guid fileId, string username);
        Task<bool> IsSharedForEveryone(Guid fileId);
        Task<bool> IsShared(Guid fileId);
        Task<StatusCode<ShareEveryoneOut>> GetShareForEveryoneByFileIdAndUser(Guid fileId, string username);
        Task<StatusCode<ShareEveryoneOut>> GetShareForEveryoneByToken(string token);
        Task<StatusCode<ShareEveryoneOut>> GetShareForEveryoneByTokenAndPassword(string token, string password);
        Task<StatusCode<ShareEveryoneOut>> ShareForEveryone(Guid fileId, string username, string password, DateTime? expirationDateTime, int? downloadLimit);
        Task<string> ShareForUser(Guid fileId, string ownerUsername, string username);
        Task<bool> CancelSharingForEveryone(Guid fileId, string username);
    }
}
