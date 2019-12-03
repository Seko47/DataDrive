using AutoMapper;
using DataDrive.DAO.Context;
using DataDrive.DAO.Helpers.Communication;
using DataDrive.DAO.Models;
using DataDrive.DAO.Models.Base;
using DataDrive.Share.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<bool> IsShared(Guid fileId)
        {
            ShareAbstract share = await _databaseContext.ShareAbstracts.FirstOrDefaultAsync(_ => _.ResourceID == fileId);

            return share != null;
        }

        public async Task<bool> IsSharedForEveryone(Guid fileId)
        {
            ShareEveryone shareEveryone = await _databaseContext.ShareEveryones.FirstOrDefaultAsync(_ => _.ResourceID == fileId);

            return shareEveryone != null;
        }

        public Task<bool> IsSharedForUser(Guid fileId, string username)
        {
            throw new NotImplementedException();
        }

        public async Task<StatusCode<ShareEveryoneOut>> GetShareForEveryoneByFileIdAndUser(Guid fileId, string username)
        {
            string userId = (await _databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == username))
                ?.Id;

            ShareEveryone shareEveryone = await _databaseContext.ShareEveryones
                .FirstOrDefaultAsync(_ => _.ResourceID == fileId && _.OwnerID == userId);

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

        public async Task<StatusCode<ShareEveryoneOut>> ShareForEveryone(Guid fileId, string username, string password, DateTime? expirationDateTime, int? downloadLimit)
        {
            string userId = (await _databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == username))?.Id;

            ResourceAbstract fileAbstract = await _databaseContext.ResourceAbstracts.FirstOrDefaultAsync(_ => _.ID == fileId && _.OwnerID == userId);

            if (fileAbstract == null)
            {
                return new StatusCode<ShareEveryoneOut>(StatusCodes.Status404NotFound, StatusMessages.FILE_NOT_FOUND);
            }

            ShareEveryone shareEveryone = await _databaseContext.ShareEveryones.FirstOrDefaultAsync(_ => _.ResourceID == fileId && _.OwnerID == userId);

            if (shareEveryone == null)
            {
                shareEveryone = new ShareEveryone
                {
                    CreatedDateTime = DateTime.Now,
                    DownloadLimit = downloadLimit,
                    ExpirationDateTime = expirationDateTime,
                    ResourceID = fileId,
                    OwnerID = userId,
                    Password = password,
                    Token = GenerateToken()
                };

                await _databaseContext.ShareEveryones.AddAsync(shareEveryone);

                fileAbstract.IsShared = true;
                fileAbstract.IsSharedForEveryone = true;
            }
            else
            {
                shareEveryone.DownloadLimit = downloadLimit;
                shareEveryone.ExpirationDateTime = expirationDateTime;
                shareEveryone.Password = password;
                shareEveryone.LastModifiedDateTime = DateTime.Now;
            }

            await _databaseContext.SaveChangesAsync();

            ShareEveryoneOut result = _mapper.Map<ShareEveryoneOut>(shareEveryone);

            return new StatusCode<ShareEveryoneOut>(StatusCodes.Status200OK, result);
        }

        public async Task<bool> CancelSharingForEveryone(Guid fileId, string username)
        {
            string userId = (await _databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == username))?.Id;

            ResourceAbstract fileAbstract = await _databaseContext.ResourceAbstracts.FirstOrDefaultAsync(_ => _.ID == fileId && _.OwnerID == userId);
            ShareEveryone shareEveryone = await _databaseContext.ShareEveryones.FirstOrDefaultAsync(_ => _.ResourceID == fileId && _.OwnerID == userId);

            if (fileAbstract == null || shareEveryone == null)
            {
                return false;
            }

            _databaseContext.ShareEveryones.Remove(shareEveryone);
            fileAbstract.IsSharedForEveryone = false;
            fileAbstract.IsShared = fileAbstract.IsSharedForUsers;

            await _databaseContext.SaveChangesAsync();

            return !fileAbstract.IsSharedForEveryone &&
                   !(await _databaseContext.ShareEveryones.AnyAsync(_ => _.ResourceID == fileId && _.OwnerID == userId));
        }


        public Task<string> ShareForUser(Guid fileId, string ownerUsername, string username)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CancelSharingForUser(Guid noteId, string userId)
        {
            throw new NotImplementedException();
        }


        private string GenerateToken()
        {
            char[] range = new char[] {'a', 'b', 'c', 'd', 'e', 'f', 'g',
                                       'h', 'i', 'j', 'k', 'l', 'm', 'n',
                                       'o', 'p', 'q', 'r', 's', 't', 'u',
                                       'v', 'w', 'x', 'y', 'z', '0', '1',
                                       '2', '3', '4', '5', '6', '7', '8',
                                       '9'};
            int rangeLength = range.Length;
            int tokenLength = 3;
            int numberOfDraws = 0;
            int drawsLimit = 3;

            Random random = new Random();

            string token;

            do
            {
                token = "";

                if (numberOfDraws >= drawsLimit)
                {
                    ++tokenLength;
                    numberOfDraws = 0;
                }

                ++numberOfDraws;

                for (int i = 0; i < tokenLength; ++i)
                {
                    token += range[random.Next(rangeLength)];
                }

            } while (_databaseContext.ShareEveryones.AnyAsync(_ => _.Token == token).Result);

            return token.ToUpper();
        }

    }
}
