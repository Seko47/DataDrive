﻿using AutoMapper;
using DataDrive.DAO.Context;
using DataDrive.DAO.Helpers.Communication;
using DataDrive.DAO.Models;
using DataDrive.DAO.Models.Base;
using DataDrive.Files.Models;
using DataDrive.Files.Models.In;
using DataDrive.Files.Models.Out;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.StaticFiles;
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
                FileType = FileType.DIRECTORY,
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

            FileAbstract fileAbstractToDelete = await _databaseContext.FileAbstracts
                .FirstOrDefaultAsync(_ => _.ID == id && _.OwnerID == userId);

            if (fileAbstractToDelete == null)
            {
                return new StatusCode<DirectoryOut>(StatusCodes.Status404NotFound, StatusMessages.FILE_NOT_FOUND);
            }

            List<ShareAbstract> shares = await _databaseContext.ShareAbstracts
                .Where(_ => _.FileID == fileAbstractToDelete.ID)
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
                    .FirstOrDefaultAsync(_ => _.ID == parentDirectoryId && _.OwnerID == userId);

                parentDirectoryOutResult = _mapper.Map<DirectoryOut>(directoryToReturn);
            }
            else
            {
                List<FileAbstract> files = await _databaseContext.FileAbstracts
                    .Include(_ => _.ParentDirectory)
                    .Where(_ => _.ParentDirectoryID == null && _.OwnerID == userId)
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

            return new StatusCode<DirectoryOut>(StatusCodes.Status200OK, StatusMessages.FILE_DELETED, parentDirectoryOutResult);
        }

        public async Task<StatusCode<DownloadFileInfo>> DownloadByIdAndUser(Guid id, string username)
        {
            string userId = (await _databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == username))?.Id;

            File fileToDownload = await _databaseContext.Files
                .Include(_ => _.ShareEveryone)
                .Include(_ => _.ShareForUsers)
                .FirstOrDefaultAsync(_ => _.ID == id);

            if (fileToDownload == null
                || (fileToDownload.OwnerID != userId
                && (!fileToDownload.IsSharedForUsers || !fileToDownload.ShareForUsers.Any(_ => _.OwnerID == userId))
                && !fileToDownload.IsSharedForEveryone))
            {
                return new StatusCode<DownloadFileInfo>(StatusCodes.Status404NotFound, StatusMessages.FILE_NOT_FOUND);
            }

            byte[] fileContent = System.IO.File.ReadAllBytes(fileToDownload.Path);

            DownloadFileInfo downloadFileInfo = new DownloadFileInfo(fileToDownload.Name, fileContent);

            return new StatusCode<DownloadFileInfo>(StatusCodes.Status200OK, downloadFileInfo);
        }

        public async Task<StatusCode<FileOut>> GetByIdAndUser(Guid id, string username)
        {
            string userId = (await _databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == username))?.Id;

            FileAbstract fileAbstract = await _databaseContext.FileAbstracts
                .Include(_ => _.ParentDirectory)
                .Include(_ => _.ShareEveryone)
                .Include(_ => _.ShareForUsers)
                .FirstOrDefaultAsync(_ => _.ID == id);

            if (fileAbstract == null
                || (fileAbstract.OwnerID != userId
                && (!fileAbstract.IsSharedForUsers || !fileAbstract.ShareForUsers.Any(_ => _.OwnerID == userId))
                && !fileAbstract.IsSharedForEveryone))
            {
                return new StatusCode<FileOut>(StatusCodes.Status404NotFound, StatusMessages.FILE_NOT_FOUND);
            }

            FileOut fileResult = _mapper.Map<FileOut>(fileAbstract);

            return new StatusCode<FileOut>(StatusCodes.Status200OK, fileResult);
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
                        FileType = FileType.DIRECTORY,
                        LastModifiedDateTime = DateTime.Now,
                        Name = "Root"
                    };
                }

                result = _mapper.Map<DirectoryOut>(directory);
            }
            else
            {
                List<FileAbstract> files = await _databaseContext.FileAbstracts
                    .Where(_ => _.ParentDirectoryID == id && _.OwnerID == userId)
                    .ToListAsync();

                result = new DirectoryOut
                {
                    CreatedDateTime = DateTime.Now,
                    Files = _mapper.Map<List<FileOut>>(files),
                    FileType = FileType.DIRECTORY,
                    LastModifiedDateTime = DateTime.Now,
                    Name = "Root"
                };
            }

            int[] sortingMap = new[] { 2, 1, 3 };
            result.Files = result.Files.OrderBy(_ => sortingMap[(int)(_.FileType)]).ThenBy(_ => _.Name).ThenBy(_ => _.ID).ToList();

            return new StatusCode<DirectoryOut>(StatusCodes.Status200OK, result);
        }

        public async Task<StatusCode<FileOut>> PatchByIdAndFilePatchAndUser(Guid id, JsonPatchDocument<FilePatch> jsonPatchDocument, string username)
        {
            string userId = (await _databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == username))?.Id;

            FileAbstract fileAbstract = await _databaseContext.FileAbstracts.FirstOrDefaultAsync(_ => _.ID == id && _.OwnerID == userId);

            if (fileAbstract == null)
            {
                return new StatusCode<FileOut>(StatusCodes.Status404NotFound, StatusMessages.FILE_NOT_FOUND);
            }

            JsonPatchDocument<FileAbstract> fileAbstractPatch = _mapper.Map<JsonPatchDocument<FileAbstract>>(jsonPatchDocument);

            fileAbstractPatch.ApplyTo(fileAbstract);

            fileAbstract.LastModifiedDateTime = DateTime.Now;

            await _databaseContext.SaveChangesAsync();

            FileOut result = _mapper.Map<FileOut>(await _databaseContext.FileAbstracts.FirstOrDefaultAsync(_ => _.ID == id));

            return new StatusCode<FileOut>(StatusCodes.Status200OK, result);
        }

        public async Task<StatusCode<List<FileUploadResult>>> PostByUser(FilePost filePost, string username)
        {
            string userId = (await _databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == username))?
                .Id;

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
                    DateTime actualTime = DateTime.Now;
                    string fileName = actualTime.Year + actualTime.Month + actualTime.Day + "_" + actualTime.Hour + actualTime.Minute + actualTime.Second + "_" + Guid.NewGuid().ToString();
                    string pathToNewFile = System.IO.Path.Combine(pathToUploadDirectory, fileName);

                    System.IO.FileStream fileStream = new System.IO.FileStream(pathToNewFile, System.IO.FileMode.Create);
                    Task fileUploadInProgres = file.CopyToAsync(fileStream);

                    File newFile = new File
                    {
                        CreatedDateTime = DateTime.Now,
                        FileType = FileType.FILE,
                        Name = file.FileName,
                        OwnerID = userId,
                        ParentDirectoryID = filePost.ParentDirectoryID,
                        Path = pathToNewFile
                    };

                    await _databaseContext.Files.AddAsync(newFile);
                    await _databaseContext.SaveChangesAsync();

                    result.Add(new FileUploadResult { Name = file.FileName, Length = file.Length });

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
