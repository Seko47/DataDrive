using AutoMapper;
using DataDrive.DAO.Context;
using DataDrive.DAO.Models;
using DataDrive.DAO.Models.Base;
using DataDrive.Share.Models;
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
            ShareAbstract share = await _databaseContext.ShareAbstracts.FirstOrDefaultAsync(_ => _.FileID == fileId);

            return share != null;
        }

        public async Task<bool> IsSharedForEveryone(Guid fileId)
        {
            ShareEveryone shareEveryone = await _databaseContext.ShareEveryones.FirstOrDefaultAsync(_ => _.FileID == fileId);

            return shareEveryone != null;
        }

        public Task<bool> IsSharedForUser(Guid fileId, string username)
        {
            throw new NotImplementedException();
        }

        public async Task<ShareEveryoneOut> ShareForEveryone(Guid fileId, string username, string password, DateTime? expirationDateTime, int? downloadLimit)
        {
            ApplicationUser user = await _databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == username);

            if (user == null)
            {
                return null;
            }

            FileAbstract fileAbstract = await _databaseContext.FileAbstracts.FirstOrDefaultAsync(_ => _.ID == fileId && _.OwnerID == user.Id);

            if (fileAbstract == null)
            {
                return null;
            }

            ShareEveryone shareEveryone = await _databaseContext.ShareEveryones.FirstOrDefaultAsync(_ => _.FileID == fileId && _.OwnerID == user.Id);

            if (shareEveryone == null)
            {
                shareEveryone = new ShareEveryone
                {
                    CreatedDateTime = DateTime.Now,
                    DownloadLimit = downloadLimit,
                    ExpirationDateTime = expirationDateTime,
                    FileID = fileId,
                    OwnerID = user.Id,
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
            }

            await _databaseContext.SaveChangesAsync();

            ShareEveryoneOut result = _mapper.Map<ShareEveryoneOut>(shareEveryone);

            return result;
        }

        public Task<string> ShareForUser(Guid fileId, string ownerUsername, string username)
        {
            throw new NotImplementedException();
        }

        public Task CancelSharingForEveryone(Guid fileId, string username)
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
                    token += random.Next(rangeLength);
                }

            } while (_databaseContext.ShareEveryones.AnyAsync(_ => _.Token == token).Result);

            return token.ToUpper();
        }
    }
}
