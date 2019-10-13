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
