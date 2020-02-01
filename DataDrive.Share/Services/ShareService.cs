using AutoMapper;
using DataDrive.DAO.Context;
using DataDrive.DAO.Helpers.Communication;
using DataDrive.DAO.Models;
using DataDrive.DAO.Models.Base;
using DataDrive.Share.Models;
using DataDrive.Share.Models.In;
using DataDrive.Share.Models.Out;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDrive.Share.Services
{
    public class ShareService : IShareService
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public ShareService(IDatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }

        public async Task<bool> IsShared(Guid resourceId)
        {
            ShareAbstract share = await _databaseContext.ShareAbstracts.FirstOrDefaultAsync(_ => _.ResourceID == resourceId);

            return share != null;
        }

        public async Task<bool> IsSharedForEveryone(Guid resourceId)
        {
            ShareEveryone shareEveryone = await _databaseContext.ShareEveryones.FirstOrDefaultAsync(_ => _.ResourceID == resourceId);

            return shareEveryone != null;
        }

        public Task<bool> IsSharedForUser(Guid resourceId, string username)
        {
            throw new NotImplementedException();
        }

        public async Task<StatusCode<ShareEveryoneOut>> GetShareForEveryoneByFileIdAndUser(Guid resourceId, string username)
        {
            string userId = (await _databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == username))
                ?.Id;

            ShareEveryone shareEveryone = await _databaseContext.ShareEveryones
                .FirstOrDefaultAsync(_ => _.ResourceID == resourceId && _.OwnerID == userId);

            if (shareEveryone == null)
            {
                return new StatusCode<ShareEveryoneOut>(StatusCodes.Status404NotFound, StatusMessages.FILE_IS_NOT_SHARED);
            }

            ShareEveryoneOut result = _mapper.Map<ShareEveryoneOut>(shareEveryone);

            return new StatusCode<ShareEveryoneOut>(StatusCodes.Status200OK, result);
        }

        public async Task<StatusCode<ShareEveryoneOut>> GetShareForEveryoneByToken(string token)
        {
            ShareEveryone shareEveryone = await _databaseContext.ShareEveryones
                .Include(_ => _.Owner)
                .Include(_ => _.Resource)
                .FirstOrDefaultAsync(_ => _.Token == token);

            if (shareEveryone == null)
            {
                return new StatusCode<ShareEveryoneOut>(StatusCodes.Status404NotFound, StatusMessages.TOKEN_NOT_FOUND);
            }

            if (!String.IsNullOrEmpty(shareEveryone.Password))
            {
                return new StatusCode<ShareEveryoneOut>(StatusCodes.Status401Unauthorized, StatusMessages.PASSWORD_REQUIRED);
            }

            ShareEveryoneOut shareEveryoneOut = _mapper.Map<ShareEveryoneOut>(shareEveryone);

            return new StatusCode<ShareEveryoneOut>(StatusCodes.Status200OK, shareEveryoneOut);
        }

        public async Task<StatusCode<ShareEveryoneOut>> GetShareForEveryoneByTokenAndPassword(string token, string password)
        {
            ShareEveryone shareEveryone = await _databaseContext.ShareEveryones
                .Include(_ => _.Owner)
                .Include(_ => _.Resource)
                .FirstOrDefaultAsync(_ => _.Token == token);

            if (shareEveryone == null)
            {
                return new StatusCode<ShareEveryoneOut>(StatusCodes.Status404NotFound, StatusMessages.TOKEN_NOT_FOUND);
            }

            if (shareEveryone.Password != password)
            {
                return new StatusCode<ShareEveryoneOut>(StatusCodes.Status401Unauthorized, StatusMessages.PASSWORD_IS_WRONG);
            }

            ShareEveryoneOut shareEveryoneOut = _mapper.Map<ShareEveryoneOut>(shareEveryone);

            return new StatusCode<ShareEveryoneOut>(StatusCodes.Status200OK, shareEveryoneOut);
        }

        public async Task<StatusCode<ShareEveryoneOut>> ShareForEveryone(Guid resourceId, string username, string password, DateTime? expirationDateTime, int? downloadLimit)
        {
            string userId = (await _databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == username))?.Id;

            ResourceAbstract resourceAbstract = await _databaseContext.ResourceAbstracts.FirstOrDefaultAsync(_ => _.ID == resourceId && _.OwnerID == userId);

            if (resourceAbstract == null)
            {
                return new StatusCode<ShareEveryoneOut>(StatusCodes.Status404NotFound, StatusMessages.FILE_NOT_FOUND);
            }

            ShareEveryone shareEveryone = await _databaseContext.ShareEveryones.FirstOrDefaultAsync(_ => _.ResourceID == resourceId && _.OwnerID == userId);

            if (shareEveryone == null)
            {
                shareEveryone = new ShareEveryone
                {
                    CreatedDateTime = DateTime.Now,
                    DownloadLimit = downloadLimit,
                    ExpirationDateTime = expirationDateTime,
                    ResourceID = resourceId,
                    OwnerID = userId,
                    Password = password,
                    Token = GenerateToken()
                };

                await _databaseContext.ShareEveryones.AddAsync(shareEveryone);

                resourceAbstract.IsShared = true;
                resourceAbstract.IsSharedForEveryone = true;
            }
            else
            {
                shareEveryone.DownloadLimit = downloadLimit;
                shareEveryone.ExpirationDateTime = expirationDateTime;
                shareEveryone.Password = password;
                shareEveryone.LastModifiedDateTime = DateTime.Now;
            }

            if (shareEveryone.ExpirationDateTime.HasValue)
            {
                shareEveryone.ExpirationDateTime = shareEveryone.ExpirationDateTime.Value.AddHours(1);
            }

            await _databaseContext.SaveChangesAsync();

            ShareEveryoneOut result = _mapper.Map<ShareEveryoneOut>(shareEveryone);

            return new StatusCode<ShareEveryoneOut>(StatusCodes.Status200OK, result);
        }

        public async Task<bool> CancelSharingForEveryone(Guid resourceId, string username)
        {
            string userId = (await _databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == username))?.Id;

            ResourceAbstract fileAbstract = await _databaseContext.ResourceAbstracts.FirstOrDefaultAsync(_ => _.ID == resourceId && _.OwnerID == userId);
            ShareEveryone shareEveryone = await _databaseContext.ShareEveryones.FirstOrDefaultAsync(_ => _.ResourceID == resourceId && _.OwnerID == userId);

            if (fileAbstract == null || shareEveryone == null)
            {
                return false;
            }

            _databaseContext.ShareEveryones.Remove(shareEveryone);
            fileAbstract.IsSharedForEveryone = false;
            fileAbstract.IsShared = fileAbstract.IsSharedForUsers;

            await _databaseContext.SaveChangesAsync();

            return !fileAbstract.IsSharedForEveryone &&
                   !(await _databaseContext.ShareEveryones.AnyAsync(_ => _.ResourceID == resourceId && _.OwnerID == userId));
        }


        public async Task<StatusCode<List<ShareForUserOut>>> GetShareForUserByResourceIdAndOwner(Guid resourceId, string ownerUsername)
        {
            string ownerId = (await _databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == ownerUsername))?
                .Id;

            List<ShareForUser> shareForUsers = await _databaseContext.ShareForUsers
                .Include(_ => _.Owner)
                .Include(_ => _.Resource)
                .Include(_ => _.SharedForUser)
                .Where(_ => _.ResourceID == resourceId && _.OwnerID == ownerId)
                .ToListAsync();

            if (shareForUsers == null || !shareForUsers.Any())
            {
                return new StatusCode<List<ShareForUserOut>>(StatusCodes.Status404NotFound, $"Share not found");
            }

            List<ShareForUserOut> result = _mapper.Map<List<ShareForUserOut>>(shareForUsers);

            return new StatusCode<List<ShareForUserOut>>(StatusCodes.Status200OK, result);
        }

        public async Task<StatusCode<List<ShareForUserOut>>> GetShareForUserByUserAndFilter(ShareFilter shareFilter, string username)
        {
            string userId = (await _databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == username))?
                .Id;

            List<ShareForUser> shareForUsers = await _databaseContext.ShareForUsers
                .Include(_ => _.Owner)
                .Include(_ => _.Resource)
                .Where(_ => _.SharedForUserID == userId && _.Resource.ResourceType == shareFilter.ResourceType
                        && (_.ExpirationDateTime == null || (_.ExpirationDateTime != null && _.ExpirationDateTime > DateTime.Now)))
                .ToListAsync();

            if (shareForUsers == null || !shareForUsers.Any())
            {
                return new StatusCode<List<ShareForUserOut>>(StatusCodes.Status404NotFound, $"Shared resources not found");
            }

            List<ShareForUserOut> result = _mapper.Map<List<ShareForUserOut>>(shareForUsers);

            return new StatusCode<List<ShareForUserOut>>(StatusCodes.Status200OK, result);
        }

        public async Task<StatusCode<ShareForUserOut>> ShareForUser(ShareForUserIn shareForUserIn, string ownerUsername)
        {
            if (shareForUserIn.Username == ownerUsername)
            {
                return new StatusCode<ShareForUserOut>(StatusCodes.Status404NotFound, $"You cannot share your own file with yourself");
            }

            if (!await _databaseContext.Users.AnyAsync(_ => _.UserName == shareForUserIn.Username))
            {
                return new StatusCode<ShareForUserOut>(StatusCodes.Status404NotFound, $"User {shareForUserIn.Username} not found");
            }

            string ownerId = (await _databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == ownerUsername))?
                .Id;

            ResourceAbstract resourceAbstract = await _databaseContext.ResourceAbstracts
                .Include(_ => _.ShareForUsers)
                .FirstOrDefaultAsync(_ => _.ID == shareForUserIn.ResourceId && _.OwnerID == ownerId);

            if (resourceAbstract == null)
            {
                return new StatusCode<ShareForUserOut>(StatusCodes.Status404NotFound, $"Resource {shareForUserIn.ResourceId} not found");
            }

            string userId = (await _databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == shareForUserIn.Username))?
                .Id;

            ShareForUser shareForUser = await _databaseContext.ShareForUsers
                .FirstOrDefaultAsync(_ => _.OwnerID == ownerId && _.ResourceID == shareForUserIn.ResourceId && _.SharedForUserID == userId);

            if (shareForUser == null)
            {
                shareForUser = new ShareForUser
                {
                    CreatedDateTime = DateTime.Now,
                    ExpirationDateTime = shareForUserIn.ExpirationDateTime,
                    OwnerID = ownerId,
                    ResourceID = shareForUserIn.ResourceId,
                    SharedForUserID = userId
                };

                resourceAbstract.IsShared = true;
                resourceAbstract.IsSharedForUsers = true;
                resourceAbstract.ShareForUsers.Add(shareForUser);
            }
            else
            {
                shareForUser.LastModifiedDateTime = DateTime.Now;
                shareForUser.ExpirationDateTime = shareForUserIn.ExpirationDateTime;
            }

            if (shareForUser.ExpirationDateTime.HasValue)
            {
                shareForUser.ExpirationDateTime = shareForUser.ExpirationDateTime.Value.AddHours(1);
            }

            await _databaseContext.SaveChangesAsync();

            ShareForUserOut result = _mapper.Map<ShareForUserOut>(shareForUser);

            return new StatusCode<ShareForUserOut>(StatusCodes.Status200OK, result);
        }

        public async Task<bool> CancelSharingForUser(Guid shareId, string ownerUsername)
        {
            string ownerId = (await _databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == ownerUsername))?
                .Id;

            ShareForUser shareForUserToDelete = await _databaseContext.ShareForUsers
                .Include(_ => _.Resource)
                .FirstOrDefaultAsync(_ => _.ID == shareId && _.OwnerID == ownerId);

            if (shareForUserToDelete == null)
            {
                return false;
            }

            shareForUserToDelete.Resource.IsSharedForUsers = false;
            shareForUserToDelete.Resource.IsShared = shareForUserToDelete.Resource.IsSharedForEveryone;

            await _databaseContext.SaveChangesAsync();

            _databaseContext.ShareForUsers.Remove(shareForUserToDelete);

            await _databaseContext.SaveChangesAsync();

            return !await _databaseContext.ShareForUsers.AnyAsync(_ => _.ID == shareId);
        }


        private string GenerateToken()
        {
            char[] chars = new char[] {'a', 'b', 'c', 'd', 'e', 'f', 'g',
                                       'h', 'i', 'j', 'k', 'l', 'm', 'n',
                                       'o', 'p', 'q', 'r', 's', 't', 'u',
                                       'v', 'w', 'x', 'y', 'z', '0', '1',
                                       '2', '3', '4', '5', '6', '7', '8',
                                       '9'};
            int tokenLength = 3;
            int numberOfDraws = 0;
            int drawsLimit = 3;

            Random random = new Random();
            string token;

            do
            {
                if (numberOfDraws >= drawsLimit)
                {
                    ++tokenLength;
                    numberOfDraws = 0;
                }

                ++numberOfDraws;

                token = new string(Enumerable.Repeat(chars, tokenLength)
                    .Select(c => c[random.Next(chars.Length)])
                    .ToArray());

            } while (_databaseContext.ShareEveryones.Any(_ => _.Token == token));

            return token.ToUpper();
        }

        public async Task<bool> ReportResource(Guid resourceId)
        {
            ResourceAbstract resource = await _databaseContext.ResourceAbstracts
                .FirstOrDefaultAsync(_ => _.IsShared && _.ID == resourceId);

            if(resource == null)
            {
                return false;
            }

            ++resource.NumberOfReports;

            await _databaseContext.SaveChangesAsync();

            return true;
        }
    }
}
