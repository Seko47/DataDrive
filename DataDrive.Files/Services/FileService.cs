using AutoMapper;
using DataDrive.DAO.Context;
using DataDrive.DAO.Helpers.Communication;
using DataDrive.DAO.Models;
using DataDrive.DAO.Models.Base;
using DataDrive.Files.Models;
using DataDrive.Files.Models.In;
using DataDrive.Files.Models.Out;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataDrive.Files.Services
{
    //TODO wyszukiwarka plików
    public class FileService : IFileService
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public FileService(IDatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }

        public async Task<StatusCode<DirectoryOut>> CreateDirectoryByUser(DirectoryPost directoryPost, string username)
        {
            string userId = (await _databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == username))?.Id;

            if (directoryPost.ParentDirectoryID != null && !_databaseContext.Directories.AnyAsync(_ => _.OwnerID == userId && _.ID == directoryPost.ParentDirectoryID).Result)
            {
                return new StatusCode<DirectoryOut>(StatusCodes.Status404NotFound, StatusMessages.PARENT_DIRECTORY_NOT_FOUND);
            }

            string nameForNewDirectory = directoryPost.Name;

            Directory newDirectory = new Directory
            {
                CreatedDateTime = DateTime.Now,
                ResourceType = ResourceType.DIRECTORY,
                Name = nameForNewDirectory,
                OwnerID = userId,
                ParentDirectoryID = directoryPost.ParentDirectoryID
            };

            await _databaseContext.Directories.AddAsync(newDirectory);

            await _databaseContext.SaveChangesAsync();

            DirectoryOut result = _mapper.Map<DirectoryOut>(newDirectory);

            return new StatusCode<DirectoryOut>(StatusCodes.Status201Created, result);
        }

        public async Task<StatusCode<DirectoryOut>> DeleteByIdAndUser(Guid id, string username)
        {
            string userId = (await _databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == username))?.Id;

            ResourceAbstract fileAbstractToDelete = await _databaseContext.ResourceAbstracts
                .FirstOrDefaultAsync(_ => _.ID == id && _.OwnerID == userId);

            if (fileAbstractToDelete == null)
            {
                return new StatusCode<DirectoryOut>(StatusCodes.Status404NotFound, StatusMessages.FILE_NOT_FOUND);
            }

            List<ShareAbstract> shares = await _databaseContext.ShareAbstracts
                .Where(_ => _.ResourceID == fileAbstractToDelete.ID)
                .ToListAsync();

            _databaseContext.ShareAbstracts.RemoveRange(shares);

            Guid? parentDirectoryId = fileAbstractToDelete.ParentDirectoryID;

            if (fileAbstractToDelete is File)
            {
                File fileToDelete = fileAbstractToDelete as File;
                System.IO.File.Delete(fileToDelete.Path);

                if (System.IO.File.Exists(fileToDelete.Path))
                {
                    return new StatusCode<DirectoryOut>(StatusCodes.Status400BadRequest, StatusMessages.CANNOT_DELETE_FILE);
                }

                _databaseContext.Files.Remove(fileToDelete);
                await _databaseContext.SaveChangesAsync();
            }
            else if (fileAbstractToDelete is Directory)
            {
                Directory directoryToDelete = await _databaseContext.Directories
                    .Include(_ => _.Files)
                    .FirstOrDefaultAsync(_ => _.ID == fileAbstractToDelete.ID && _.OwnerID == userId);

                foreach (ResourceAbstract fileAbstract in directoryToDelete.Files.ToList())
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
                    .FirstOrDefaultAsync(_ => _.ID == parentDirectoryId && _.OwnerID == userId);

                parentDirectoryOutResult = _mapper.Map<DirectoryOut>(directoryToReturn);
            }
            else
            {
                List<ResourceAbstract> files = await _databaseContext.ResourceAbstracts
                    .Include(_ => _.ParentDirectory)
                    .Where(_ => _.ParentDirectoryID == null && _.OwnerID == userId)
                    .ToListAsync();

                parentDirectoryOutResult = new DirectoryOut
                {
                    ID = null,
                    Files = _mapper.Map<List<FileOut>>(files),
                    ResourceType = ResourceType.DIRECTORY,
                    Name = "root",
                    ParentDirectoryID = null,
                    ParentDirectoryName = null
                };
            }

            return new StatusCode<DirectoryOut>(StatusCodes.Status200OK, StatusMessages.FILE_DELETED, parentDirectoryOutResult);
        }

        public async Task<StatusCode<DownloadFileInfo>> DownloadByIdAndUser(Guid id, string username)
        {
            string userId = (await _databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == username))?.Id;

            File fileToDownload = await _databaseContext.Files
                .Include(_ => _.ShareEveryone)
                .Include(_ => _.ShareForUsers)
                .FirstOrDefaultAsync(_ => _.ID == id);
            
            if (fileToDownload == null)
            {
                return new StatusCode<DownloadFileInfo>(StatusCodes.Status404NotFound, $"File {id} not found");
            }

            if (fileToDownload.OwnerID == userId)
            {
                byte[] fileContent = System.IO.File.ReadAllBytes(fileToDownload.Path);

                DownloadFileInfo downloadFileInfo = new DownloadFileInfo(fileToDownload.Name, fileContent);

                return new StatusCode<DownloadFileInfo>(StatusCodes.Status200OK, downloadFileInfo);
            }

            if (fileToDownload.IsShared)
            {
                if (fileToDownload.IsSharedForEveryone)
                {
                    ShareEveryone share = fileToDownload.ShareEveryone;

                    if (share != null)
                    {
                        if ((share.ExpirationDateTime == null
                                || (share.ExpirationDateTime != null && share.ExpirationDateTime >= DateTime.Now))
                           && (share.DownloadLimit == null
                                || (share.DownloadLimit != null && share.DownloadLimit > 0)))
                        {
                            if (share.DownloadLimit != null && share.DownloadLimit > 0)
                            {
                                --share.DownloadLimit;
                                await _databaseContext.SaveChangesAsync();
                                //TODO sprawdź czy działa limit pobierania i dodaj jego obsługę do notatek
                            }

                            byte[] fileContent = System.IO.File.ReadAllBytes(fileToDownload.Path);

                            DownloadFileInfo downloadFileInfo = new DownloadFileInfo(fileToDownload.Name, fileContent);

                            return new StatusCode<DownloadFileInfo>(StatusCodes.Status200OK, downloadFileInfo);
                        }
                    }
                }
                else if (fileToDownload.IsSharedForUsers)
                {
                    ShareForUser share = fileToDownload.ShareForUsers.FirstOrDefault(_ => _.SharedForUserID == userId && _.ResourceID == id);

                    if (share != null)
                    {
                        if (share.ExpirationDateTime == null
                            || (share.ExpirationDateTime != null && share.ExpirationDateTime >= DateTime.Now))
                        {
                            byte[] fileContent = System.IO.File.ReadAllBytes(fileToDownload.Path);

                            DownloadFileInfo downloadFileInfo = new DownloadFileInfo(fileToDownload.Name, fileContent);

                            return new StatusCode<DownloadFileInfo>(StatusCodes.Status200OK, downloadFileInfo);
                        }
                    }
                }
            }

            return new StatusCode<DownloadFileInfo>(StatusCodes.Status404NotFound, $"File {id} not found");
        }

        public async Task<StatusCode<FileOut>> GetByIdAndUser(Guid id, string username)
        {
            string userId = (await _databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == username))?.Id;

            ResourceAbstract fileAbstract = await _databaseContext.ResourceAbstracts
                .Include(_ => _.ParentDirectory)
                .Include(_ => _.ShareEveryone)
                .Include(_ => _.ShareForUsers)
                .FirstOrDefaultAsync(_ => _.ID == id && (_.ResourceType == ResourceType.FILE || _.ResourceType == ResourceType.DIRECTORY));

            if (fileAbstract == null)
            {
                return new StatusCode<FileOut>(StatusCodes.Status404NotFound, $"File {id} not found");
            }

            if (fileAbstract.OwnerID == userId)
            {
                return new StatusCode<FileOut>(StatusCodes.Status200OK, _mapper.Map<FileOut>(fileAbstract));
            }

            if (fileAbstract.IsShared)
            {
                if (fileAbstract.IsSharedForEveryone)
                {
                    ShareEveryone share = fileAbstract.ShareEveryone;

                    if (share != null)
                    {
                        if ((share.ExpirationDateTime == null
                                || (share.ExpirationDateTime != null && share.ExpirationDateTime >= DateTime.Now))
                           && (share.DownloadLimit == null
                                || (share.DownloadLimit != null && share.DownloadLimit > 0)))
                        {
                            return new StatusCode<FileOut>(StatusCodes.Status200OK, _mapper.Map<FileOut>(fileAbstract));
                        }
                    }
                }
                else if (fileAbstract.IsSharedForUsers)
                {
                    ShareForUser share = fileAbstract.ShareForUsers.FirstOrDefault(_ => _.SharedForUserID == userId && _.ResourceID == id);

                    if (share != null)
                    {
                        if (share.ExpirationDateTime == null
                            || (share.ExpirationDateTime != null && share.ExpirationDateTime >= DateTime.Now))
                        {
                            return new StatusCode<FileOut>(StatusCodes.Status200OK, _mapper.Map<FileOut>(fileAbstract));
                        }
                    }
                }
            }

            return new StatusCode<FileOut>(StatusCodes.Status404NotFound, $"File {id} not found");
        }

        public async Task<StatusCode<DirectoryOut>> GetDirectoryByIdAndUser(Guid? id, string username)
        {
            string userId = (await _databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == username))?.Id;

            DirectoryOut result;

            if (id != null)
            {
                Directory directory = await _databaseContext.Directories
                    .Include(_ => _.Files)
                    .Include(_ => _.ParentDirectory)
                    .FirstOrDefaultAsync(_ => _.ID == id && _.OwnerID == userId);

                if (directory == null)
                {
                    return new StatusCode<DirectoryOut>(StatusCodes.Status404NotFound, StatusMessages.DIRECTORY_NOT_FOUND);
                }

                if (directory.ParentDirectory == null)
                {
                    directory.ParentDirectory = new Directory
                    {
                        CreatedDateTime = DateTime.Now,
                        ResourceType = ResourceType.DIRECTORY,
                        LastModifiedDateTime = DateTime.Now,
                        Name = "Root"
                    };
                }

                result = _mapper.Map<DirectoryOut>(directory);
            }
            else
            {
                List<ResourceAbstract> files = await _databaseContext.ResourceAbstracts
                    .Where(_ => _.ParentDirectoryID == id && _.OwnerID == userId 
                        && (_.ResourceType == ResourceType.DIRECTORY || _.ResourceType == ResourceType.FILE))
                    .ToListAsync();

                result = new DirectoryOut
                {
                    CreatedDateTime = DateTime.Now,
                    Files = _mapper.Map<List<FileOut>>(files),
                    ResourceType = ResourceType.DIRECTORY,
                    LastModifiedDateTime = DateTime.Now,
                    Name = "Root"
                };
            }

            int[] sortingMap = new[] { 2, 1, 3 };
            result.Files = result.Files.OrderBy(_ => sortingMap[(int)(_.ResourceType)]).ThenBy(_ => _.Name).ThenBy(_ => _.ID).ToList();

            return new StatusCode<DirectoryOut>(StatusCodes.Status200OK, result);
        }

        public async Task<StatusCode<FileOut>> PatchByIdAndFilePatchAndUser(Guid id, JsonPatchDocument<FilePatch> jsonPatchDocument, string username)
        {
            string userId = (await _databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == username))?.Id;

            ResourceAbstract fileAbstract = await _databaseContext.ResourceAbstracts.FirstOrDefaultAsync(_ => _.ID == id && _.OwnerID == userId);

            if (fileAbstract == null)
            {
                return new StatusCode<FileOut>(StatusCodes.Status404NotFound, StatusMessages.FILE_NOT_FOUND);
            }

            JsonPatchDocument<ResourceAbstract> fileAbstractPatch = _mapper.Map<JsonPatchDocument<ResourceAbstract>>(jsonPatchDocument);

            fileAbstractPatch.ApplyTo(fileAbstract);

            fileAbstract.LastModifiedDateTime = DateTime.Now;

            await _databaseContext.SaveChangesAsync();

            FileOut result = _mapper.Map<FileOut>(await _databaseContext.ResourceAbstracts.FirstOrDefaultAsync(_ => _.ID == id));

            return new StatusCode<FileOut>(StatusCodes.Status200OK, result);
        }

        public async Task<StatusCode<List<FileUploadResult>>> PostByUser(FilePost filePost, string username)
        {
            ApplicationUser user = await _databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == username);
            string userId = user?.Id;

            Directory directory = await _databaseContext.Directories
                .FirstOrDefaultAsync(_ => _.ID == filePost.ParentDirectoryID && _.OwnerID == userId);

            if (directory == null && filePost.ParentDirectoryID != null)
            {
                return new StatusCode<List<FileUploadResult>>(StatusCodes.Status404NotFound, StatusMessages.PARENT_DIRECTORY_NOT_FOUND);
            }

            try
            {
                List<FileUploadResult> result = new List<FileUploadResult>();

                string pathToWwwroot = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot");
                if (!System.IO.Directory.Exists(pathToWwwroot))
                {
                    System.IO.Directory.CreateDirectory(pathToWwwroot);
                }

                string pathToUploadDirectory = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!System.IO.Directory.Exists(pathToUploadDirectory))
                {
                    System.IO.Directory.CreateDirectory(pathToUploadDirectory);
                }

                foreach (IFormFile file in filePost.Files)
                {
                    if(user.FreeDiskSpace < (ulong)file.Length)
                    {
                        result.Add(new FileUploadResult { Name = file.FileName, Length = file.Length, Message = "NOT_ENOUGHT_SPACE" });
                        continue;
                    }

                    DateTime actualTime = DateTime.Now;
                    string fileName = actualTime.Year + actualTime.Month + actualTime.Day + "_" + actualTime.Hour + actualTime.Minute + actualTime.Second + "_" + Guid.NewGuid().ToString();
                    string pathToNewFile = System.IO.Path.Combine(pathToUploadDirectory, fileName);

                    System.IO.FileStream fileStream = new System.IO.FileStream(pathToNewFile, System.IO.FileMode.Create);
                    Task fileUploadInProgres = file.CopyToAsync(fileStream);

                    File newFile = new File
                    {
                        CreatedDateTime = DateTime.Now,
                        ResourceType = ResourceType.FILE,
                        Name = file.FileName,
                        OwnerID = userId,
                        ParentDirectoryID = filePost.ParentDirectoryID,
                        Path = pathToNewFile
                    };

                    await _databaseContext.Files.AddAsync(newFile);

                    user.UsedDiskSpace += (ulong)file.Length;
                    await _databaseContext.SaveChangesAsync();

                    result.Add(new FileUploadResult { Name = file.FileName, Length = file.Length, Message = "UPLOADED" });

                    fileStream.Close();
                }

                return new StatusCode<List<FileUploadResult>>(StatusCodes.Status200OK, result);
            }
            catch
            {
                return new StatusCode<List<FileUploadResult>>(StatusCodes.Status500InternalServerError, StatusMessages.FAILED_TO_SAVE_FILES);
            }

        }
    }
}
