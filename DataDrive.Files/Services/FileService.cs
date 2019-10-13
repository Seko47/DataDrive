using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataDrive.DAO.Context;
using DataDrive.DAO.Models;
using DataDrive.Files.Models.In;
using DataDrive.Files.Models.Out;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace DataDrive.Files.Services
{
    public class FileService : IFileService
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public FileService(IDatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }

        public async Task<DirectoryOut> CreateDirectoryByUser(DirectoryPost directoryPost, string username)
        {
            ApplicationUser user = await _databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == username);

            if (user == null)
            {
                return null;
            }

            if (directoryPost.ParentDirectoryID != null && !_databaseContext.Directories.AnyAsync(_ => _.OwnerID == user.Id && _.ID == directoryPost.ParentDirectoryID).Result)
            {
                return null;
            }

            string nameForNewDirectory = directoryPost.Name;

            Directory newDirectory = new Directory
            {
                CreatedDateTime = DateTime.Now,
                FileType = DAO.Models.Base.FileType.DIRECTORY,
                Name = nameForNewDirectory,
                OwnerID = user.Id,
                ParentDirectoryID = directoryPost.ParentDirectoryID
            };

            await _databaseContext.Directories.AddAsync(newDirectory);

            await _databaseContext.SaveChangesAsync();

            Directory directory = await _databaseContext.Directories
                .Include(_ => _.Files)
                .FirstOrDefaultAsync(_ => _.ID == newDirectory.ID);

            DirectoryOut result = _mapper.Map<DirectoryOut>(directory);

            return result;

        }

        public Task<FileOut> DeleteByIdAndUser(Guid id, string username)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<string, byte[], string>> DownloadByIdAndUser(Guid id, string username)
        {
            throw new NotImplementedException();
        }

        public Task<FileOut> GetByIdAndUser(Guid id, string username)
        {
            throw new NotImplementedException();
        }

        public Task<DirectoryOut> GetDirectoryByIdAndUser(Guid id, string username)
        {
            throw new NotImplementedException();
        }

        public Task<FileOut> PatchByIdAndFilePatchAndUser(Guid id, JsonPatchDocument<FilePatch> jsonPatchDocument, string username)
        {
            throw new NotImplementedException();
        }

        public Task<List<FileUploadResult>> PostByUser(FilePost filePost, string username)
        {
            throw new NotImplementedException();
        }
    }
}
