using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataDrive.DAO.Context;
using DataDrive.DAO.Models;
using DataDrive.DAO.Models.Base;
using DataDrive.Files.Models.In;
using DataDrive.Files.Models.Out;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.StaticFiles;
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

            if (user == null
                || (directoryPost.ParentDirectoryID != null && !_databaseContext.Directories.AnyAsync(_ => _.OwnerID == user.Id && _.ID == directoryPost.ParentDirectoryID).Result)
               )
            {
                return null;
            }

            string nameForNewDirectory = directoryPost.Name;

            Directory newDirectory = new Directory
            {
                CreatedDateTime = DateTime.Now,
                FileType = FileType.DIRECTORY,
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

        public async Task<DirectoryOut> DeleteByIdAndUser(Guid id, string username)
        {
            string userId = (await _databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == username))?.Id;
            FileAbstract fileAbstractToDelete = await _databaseContext.FileAbstracts
                .FirstOrDefaultAsync(_ => _.ID == id && _.OwnerID == userId);

            if (fileAbstractToDelete == null)
            {
                return null;
            }

            Guid? parentDirectoryId = fileAbstractToDelete.ParentDirectoryID;

            if (fileAbstractToDelete is File)
            {
                File fileToDelete = fileAbstractToDelete as File;
                System.IO.File.Delete(fileToDelete.Path);

                if (System.IO.File.Exists(fileToDelete.Path))
                {
                    return null;
                }

                _databaseContext.Files.Remove(fileToDelete);
                await _databaseContext.SaveChangesAsync();
            }
            else if (fileAbstractToDelete is Directory)
            {
                Directory directoryToDelete = await _databaseContext.Directories
                    .Include(_ => _.Files)
                    .FirstOrDefaultAsync(_ => _.ID == fileAbstractToDelete.ID);

                foreach (FileAbstract fileAbstract in directoryToDelete.Files.ToList())
                {
                    await DeleteByIdAndUser(fileAbstract.ID, username);
                }

                _databaseContext.Directories.Remove(directoryToDelete);
                await _databaseContext.SaveChangesAsync();
            }

            DirectoryOut parentDirectoryOutResult = null;

            if (parentDirectoryId != null)
            {
                Directory directoryToReturn = await _databaseContext.Directories
                    .Include(_ => _.Files)
                    .Include(_ => _.ParentDirectory)
                    .FirstOrDefaultAsync(_ => _.ID == parentDirectoryId);

                parentDirectoryOutResult = _mapper.Map<DirectoryOut>(directoryToReturn);
            }
            else
            {
                List<FileAbstract> files = await _databaseContext.FileAbstracts
                    .Include(_ => _.ParentDirectory)
                    .Where(_ => _.ParentDirectoryID == null)
                    .ToListAsync();

                parentDirectoryOutResult = new DirectoryOut
                {
                    ID = null,
                    Files = _mapper.Map<List<FileOut>>(files),
                    FileType = FileType.DIRECTORY,
                    Name = "root",
                    ParentDirectoryID = null,
                    ParentDirectoryName = null
                };
            }

            return parentDirectoryOutResult;
        }

        public async Task<Tuple<string, byte[], string>> DownloadByIdAndUser(Guid id, string username)
        {
            string userId = (await _databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == username))?.Id;
            File fileToDownload = await _databaseContext.Files.FirstOrDefaultAsync(_ => _.ID == id && _.OwnerID == userId);

            if (fileToDownload == null)
            {
                return null;
            }

            //Get MIME type
            FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileToDownload.Name, out string contentType))
            {
                contentType = "application/octet-stream";
            }

            byte[] fileContent = System.IO.File.ReadAllBytes(fileToDownload.Path);

            return new Tuple<string, byte[], string>(fileToDownload.Name, fileContent, contentType);
        }

        public async Task<FileOut> GetByIdAndUser(Guid id, string username)
        {
            string userId = (await _databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == username))?.Id;

            FileAbstract fileAbstract = await _databaseContext.FileAbstracts
                .Include(_ => _.ParentDirectory)
                .FirstOrDefaultAsync(_ => _.ID == id && _.OwnerID == userId);

            if (fileAbstract == null)
            {
                return null;
            }

            FileOut fileResult = _mapper.Map<FileOut>(fileAbstract);
            return fileResult;
        }

        public async Task<DirectoryOut> GetDirectoryByIdAndUser(Guid id, string username)
        {
            string userId = (await _databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == username))?.Id;
            Directory directory = await _databaseContext.Directories
                .Include(_ => _.Files)
                .Include(_ => _.ParentDirectory)
                .FirstOrDefaultAsync(_ => _.ID == id && _.OwnerID == userId);

            if (directory == null)
            {
                return null;
            }

            DirectoryOut result = _mapper.Map<DirectoryOut>(directory);
            return result;
        }

        public async Task<FileOut> PatchByIdAndFilePatchAndUser(Guid id, JsonPatchDocument<FilePatch> jsonPatchDocument, string username)
        {
            string userId = (await _databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == username))?.Id;

            FileAbstract fileAbstract = await _databaseContext.FileAbstracts.FirstOrDefaultAsync(_ => _.ID == id && _.OwnerID == userId);

            if (fileAbstract == null)
            {
                return null;
            }

            JsonPatchDocument<FileAbstract> fileAbstractPatch = _mapper.Map<JsonPatchDocument<FileAbstract>>(jsonPatchDocument);

            fileAbstractPatch.ApplyTo(fileAbstract);

            await _databaseContext.SaveChangesAsync();

            FileOut result = _mapper.Map<FileOut>(await _databaseContext.FileAbstracts.FirstOrDefaultAsync(_ => _.ID == id));

            return result;
        }

        public async Task<List<FileUploadResult>> PostByUser(FilePost filePost, string username)
        {
            throw new NotImplementedException();
        }
    }
}
