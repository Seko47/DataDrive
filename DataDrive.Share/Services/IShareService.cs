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
        Task<ShareEveryoneOut> ShareForEveryone(Guid fileId, string username, string password, DateTime? expirationDateTime, int? downloadLimit);
        Task<string> ShareForUser(Guid fileId, string ownerUsername, string username);
        Task<bool> CancelSharingForEveryone(Guid fileId, string username);
    }
}
