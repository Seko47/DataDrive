using AutoMapper;
using DataDrive.DAO.Context;
using DataDrive.DAO.Models;
using DataDrive.Files.Models.In;
using DataDrive.Files.Models.Out;
using DataDrive.Files.Services;
using DataDrive.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace DataDrive.Tests.DataDrive.Files.Services
{
    public class FileServiceTest_CreateDirectoryByUser
    {
        [Fact]
        public async void Returns_DirectoryOut_when_Created()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Directory_to_DirectoryOut());
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            IFileService fileService = new FileService(databaseContext, mapper);

            DirectoryPost directoryPost = new DirectoryPost
            {
                ParentDirectoryID = null,
                Name = "TestDirectory"
            };

            DirectoryOut result = await fileService.CreateDirectoryByUser(directoryPost, "admin@admin.com");
            var a = databaseContext.Users.ToList();
            Assert.NotNull(result);
            Assert.True(result.ParentDirectoryID == directoryPost.ParentDirectoryID);
            Assert.True(result.Name == directoryPost.Name);
            Assert.True(databaseContext.Directories.Any(_ => _.ID == result.ID));
        }

        [Fact]
        public async void Returns_Null_when_ParentDirectoryNotExistOrNotBelongsToUser()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Directory_to_DirectoryOut());
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            IFileService fileService = new FileService(databaseContext, mapper);

            DirectoryPost directoryPost = new DirectoryPost
            {
                ParentDirectoryID = Guid.NewGuid(),
                Name = "TestDirectory"
            };

            DirectoryOut result = await fileService.CreateDirectoryByUser(directoryPost, "admin@admin.com");

            Assert.Null(result);
        }
    }

    public class FileServiceTest_DeleteByIdAndUser
    {
        private readonly string _pathToUpload;

        public FileServiceTest_DeleteByIdAndUser()
        {
            _pathToUpload = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestUpload");

            if (!System.IO.Directory.Exists(_pathToUpload))
            {
                System.IO.Directory.CreateDirectory(_pathToUpload);
            }
        }

        [Fact]
        public async void Returns_ParentDirectory_whenSuccessDeletedFile()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Directory_to_DirectoryOut());
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            IFileService fileService = new FileService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            Directory baseDirectory = new Directory
            {
                FileType = DAO.Models.Base.FileType.DIRECTORY,
                Name = "TestDirectory",
                OwnerID = userId
            };

            await databaseContext.Directories.AddAsync(baseDirectory);
            await databaseContext.SaveChangesAsync();

            string pathToFile = System.IO.Path.Combine(_pathToUpload, Guid.NewGuid().ToString());
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes("Test file"));
            System.IO.FileStream fileStream = new System.IO.FileStream(pathToFile, System.IO.FileMode.Create);
            await memoryStream.CopyToAsync(fileStream);
            fileStream.Close();

            Assert.True(System.IO.File.Exists(pathToFile));

            File fileToDelete = new File
            {
                FileType = DAO.Models.Base.FileType.FILE,
                Name = "ToDelete.txt",
                OwnerID = userId,
                ParentDirectoryID = baseDirectory.ID,
                Path = pathToFile,
                CreatedDateTime = DateTime.Now
            };

            await databaseContext.Files.AddAsync(fileToDelete);
            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Files.Any(_ => _.ID == fileToDelete.ID));

            DirectoryOut parentDirectoryResult = await fileService.DeleteByIdAndUser(fileToDelete.ID, "admin@admin.com");

            Assert.NotNull(parentDirectoryResult);
            Assert.True(parentDirectoryResult.ID == baseDirectory.ID);
            Assert.False(databaseContext.Files.Any(_ => _.ID == fileToDelete.ID));
            Assert.False(System.IO.File.Exists(pathToFile));
        }

        [Fact]
        public async void Returns_ParentDirectory_whenSuccessDeletedDirectory()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Directory_to_DirectoryOut());
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            IFileService fileService = new FileService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            Directory directoryToDelete = new Directory
            {
                FileType = DAO.Models.Base.FileType.DIRECTORY,
                Name = "ToDeleteDirectory",
                OwnerID = userId
            };

            await databaseContext.Directories.AddAsync(directoryToDelete);
            await databaseContext.SaveChangesAsync();

            string pathToFile = System.IO.Path.Combine(_pathToUpload, Guid.NewGuid().ToString());
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes("Test file"));
            System.IO.FileStream fileStream = new System.IO.FileStream(pathToFile, System.IO.FileMode.Create);
            await memoryStream.CopyToAsync(fileStream);
            fileStream.Close();

            Assert.True(System.IO.File.Exists(pathToFile));

            File file1InDirectoryToDelete = new File
            {
                FileType = DAO.Models.Base.FileType.FILE,
                Name = "file1.txt",
                OwnerID = userId,
                ParentDirectoryID = directoryToDelete.ID,
                Path = pathToFile,
                CreatedDateTime = DateTime.Now
            };

            await databaseContext.Files.AddAsync(file1InDirectoryToDelete);
            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Files.Any(_ => _.ID == file1InDirectoryToDelete.ID));

            Directory directoryInDirectoryToDelete = new Directory
            {
                FileType = DAO.Models.Base.FileType.DIRECTORY,
                Name = "Directory1",
                OwnerID = userId,
                ParentDirectoryID = directoryToDelete.ID,
                CreatedDateTime = DateTime.Now
            };

            await databaseContext.Directories.AddAsync(directoryInDirectoryToDelete);
            await databaseContext.SaveChangesAsync();

            string pathToFile2 = System.IO.Path.Combine(_pathToUpload, Guid.NewGuid().ToString());
            memoryStream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes("Test file2"));
            fileStream = new System.IO.FileStream(pathToFile2, System.IO.FileMode.Create);
            await memoryStream.CopyToAsync(fileStream);
            fileStream.Close();

            Assert.True(System.IO.File.Exists(pathToFile));

            File file2InDirectoryToDelete = new File
            {
                FileType = DAO.Models.Base.FileType.FILE,
                Name = "file2.txt",
                OwnerID = userId,
                ParentDirectoryID = directoryInDirectoryToDelete.ID,
                Path = pathToFile2,
                CreatedDateTime = DateTime.Now
            };

            await databaseContext.Files.AddAsync(file2InDirectoryToDelete);
            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Files.Any(_ => _.ID == file2InDirectoryToDelete.ID));

            DirectoryOut parentDirectoryResult = await fileService.DeleteByIdAndUser(directoryToDelete.ID, "admin@admin.com");

            Assert.NotNull(parentDirectoryResult);
            Assert.True(parentDirectoryResult.ID == null);
            Assert.False(databaseContext.Files.Any(_ => _.ID == file1InDirectoryToDelete.ID));
            Assert.False(System.IO.File.Exists(pathToFile));

            Assert.False(databaseContext.Files.Any(_ => _.ID == file2InDirectoryToDelete.ID));
            Assert.False(System.IO.File.Exists(pathToFile2));

            Assert.False(databaseContext.Files.Any(_ => _.ID == directoryToDelete.ID));
            Assert.False(databaseContext.Files.Any(_ => _.ID == directoryInDirectoryToDelete.ID));
        }

        [Fact]
        public async void Returns_Null_whenFileNotExist()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Directory_to_DirectoryOut());
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            IFileService fileService = new FileService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            Directory baseDirectory = new Directory
            {
                FileType = DAO.Models.Base.FileType.DIRECTORY,
                Name = "TestDirectory",
                OwnerID = userId
            };

            await databaseContext.Directories.AddAsync(baseDirectory);
            await databaseContext.SaveChangesAsync();

            DirectoryOut parentDirectoryResult = await fileService.DeleteByIdAndUser(Guid.NewGuid(), "admin@admin.com");

            Assert.Null(parentDirectoryResult);
        }

        [Fact]
        public async void Returns_Null_whenFileNotBelongsToUser()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Directory_to_DirectoryOut());
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            IFileService fileService = new FileService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            Directory baseDirectory = new Directory
            {
                FileType = DAO.Models.Base.FileType.DIRECTORY,
                Name = "TestDirectory",
                OwnerID = userId
            };

            await databaseContext.Directories.AddAsync(baseDirectory);
            await databaseContext.SaveChangesAsync();

            string pathToFile = System.IO.Path.Combine(_pathToUpload, Guid.NewGuid().ToString());
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes("Test file"));
            System.IO.FileStream fileStream = new System.IO.FileStream(pathToFile, System.IO.FileMode.Create);
            await memoryStream.CopyToAsync(fileStream);
            fileStream.Close();

            Assert.True(System.IO.File.Exists(pathToFile));

            File fileToDelete = new File
            {
                FileType = DAO.Models.Base.FileType.FILE,
                Name = "ToDelete.txt",
                OwnerID = userId,
                ParentDirectoryID = baseDirectory.ID,
                Path = pathToFile,
                CreatedDateTime = DateTime.Now
            };

            await databaseContext.Files.AddAsync(fileToDelete);
            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Files.Any(_ => _.ID == fileToDelete.ID));

            DirectoryOut parentDirectoryResult = await fileService.DeleteByIdAndUser(fileToDelete.ID, "user@user.com");

            Assert.Null(parentDirectoryResult);
            Assert.True(databaseContext.Files.Any(_ => _.ID == fileToDelete.ID));
            Assert.True(System.IO.File.Exists(pathToFile));
        }
    }
}
