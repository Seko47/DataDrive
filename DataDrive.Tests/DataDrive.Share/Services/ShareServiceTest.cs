using AutoMapper;
using DataDrive.DAO.Context;
using DataDrive.DAO.Models;
using DataDrive.Share.Models;
using DataDrive.Share.Services;
using DataDrive.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace DataDrive.Tests.DataDrive.Share.Services
{
    public class ShareServiceTest_IsShared
    {
        [Fact]
        public async void Returns_True_when_DirectoryIsShared()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();

            IShareService shareService = new ShareService(databaseContext, null);
            string ownerUsername = "admin@admin.com";
            ApplicationUser owner = await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ownerUsername);

            Assert.NotNull(owner);

            Directory directory = new Directory
            {
                ParentDirectoryID = null,
                Name = "TestDirectory",
                OwnerID = owner.Id,
                IsShared = true,
                FileType = DAO.Models.Base.FileType.DIRECTORY
            };

            await databaseContext.Directories.AddAsync(directory);
            ShareEveryone shareDirectory = new ShareEveryone
            {
                CreatedDateTime = DateTime.Now,
                FileID = directory.ID,
                OwnerID = owner.Id,
                Token = "dir_token"
            };

            await databaseContext.ShareEveryones.AddAsync(shareDirectory);

            await databaseContext.SaveChangesAsync();

            bool isSharedForEveryone = await shareService.IsSharedForEveryone(directory.ID);

            Assert.True(isSharedForEveryone);
        }

        [Fact]
        public async void Returns_True_when_FileIsShared()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();

            IShareService shareService = new ShareService(databaseContext, null);
            string ownerUsername = "admin@admin.com";
            ApplicationUser owner = await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ownerUsername);

            Assert.NotNull(owner);

            File file = new File
            {
                ParentDirectoryID = null,
                Name = "TestFile.txt",
                OwnerID = owner.Id,
                IsShared = true,
                FileType = DAO.Models.Base.FileType.FILE
            };

            await databaseContext.Files.AddAsync(file);
            ShareEveryone shareFile = new ShareEveryone
            {
                CreatedDateTime = DateTime.Now,
                FileID = file.ID,
                OwnerID = owner.Id,
                Token = "file_token"
            };

            await databaseContext.ShareEveryones.AddAsync(shareFile);

            await databaseContext.SaveChangesAsync();

            bool isSharedForEveryone = await shareService.IsSharedForEveryone(file.ID);

            Assert.True(isSharedForEveryone);
        }

        [Fact]
        public async void Returns_False_when_DirectoryIsNotShared()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();

            IShareService shareService = new ShareService(databaseContext, null);
            string ownerUsername = "admin@admin.com";
            ApplicationUser owner = await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ownerUsername);

            Assert.NotNull(owner);

            Directory directory = new Directory
            {
                ParentDirectoryID = null,
                Name = "TestDirectory",
                OwnerID = owner.Id,
                IsShared = true,
                FileType = DAO.Models.Base.FileType.DIRECTORY
            };

            await databaseContext.Directories.AddAsync(directory);

            await databaseContext.SaveChangesAsync();

            bool isSharedForEveryone = await shareService.IsSharedForEveryone(directory.ID);

            Assert.False(isSharedForEveryone);
        }

        [Fact]
        public async void Returns_False_when_FileIsNotShared()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();

            IShareService shareService = new ShareService(databaseContext, null);
            string ownerUsername = "admin@admin.com";
            ApplicationUser owner = await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ownerUsername);

            Assert.NotNull(owner);

            File file = new File
            {
                ParentDirectoryID = null,
                Name = "TestFile.txt",
                OwnerID = owner.Id,
                IsShared = true,
                FileType = DAO.Models.Base.FileType.FILE
            };

            await databaseContext.Files.AddAsync(file);

            await databaseContext.SaveChangesAsync();

            bool isSharedForEveryone = await shareService.IsSharedForEveryone(file.ID);

            Assert.False(isSharedForEveryone);
        }
    }

    public class ShareServiceTest_IsSharedForEveryone
    {
        [Fact]
        public async void Returns_True_when_DirectoryIsSharedForEveryone()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();

            IShareService shareService = new ShareService(databaseContext, null);
            string ownerUsername = "admin@admin.com";
            ApplicationUser owner = await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ownerUsername);

            Assert.NotNull(owner);

            Directory directory = new Directory
            {
                ParentDirectoryID = null,
                Name = "TestDirectory",
                OwnerID = owner.Id,
                IsShared = true,
                IsSharedForEveryone = true,
                FileType = DAO.Models.Base.FileType.DIRECTORY
            };

            await databaseContext.Directories.AddAsync(directory);
            ShareEveryone shareDirectory = new ShareEveryone
            {
                CreatedDateTime = DateTime.Now,
                FileID = directory.ID,
                OwnerID = owner.Id,
                Token = "dir_token"
            };

            await databaseContext.ShareEveryones.AddAsync(shareDirectory);

            await databaseContext.SaveChangesAsync();

            bool isSharedForEveryone = await shareService.IsSharedForEveryone(directory.ID);

            Assert.True(isSharedForEveryone);
        }

        [Fact]
        public async void Returns_True_when_FileIsSharedForEveryone()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();

            IShareService shareService = new ShareService(databaseContext, null);
            string ownerUsername = "admin@admin.com";
            ApplicationUser owner = await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ownerUsername);

            Assert.NotNull(owner);

            File file = new File
            {
                ParentDirectoryID = null,
                Name = "TestFile.txt",
                OwnerID = owner.Id,
                IsShared = true,
                IsSharedForEveryone = true,
                FileType = DAO.Models.Base.FileType.FILE
            };

            await databaseContext.Files.AddAsync(file);
            ShareEveryone shareFile = new ShareEveryone
            {
                CreatedDateTime = DateTime.Now,
                FileID = file.ID,
                OwnerID = owner.Id,
                Token = "file_token"
            };

            await databaseContext.ShareEveryones.AddAsync(shareFile);

            await databaseContext.SaveChangesAsync();

            bool isSharedForEveryone = await shareService.IsSharedForEveryone(file.ID);

            Assert.True(isSharedForEveryone);
        }

        [Fact]
        public async void Returns_False_when_DirectoryIsNotSharedForEveryone()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();

            IShareService shareService = new ShareService(databaseContext, null);
            string ownerUsername = "admin@admin.com";
            ApplicationUser owner = await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ownerUsername);

            Assert.NotNull(owner);

            Directory directory = new Directory
            {
                ParentDirectoryID = null,
                Name = "TestDirectory",
                OwnerID = owner.Id,
                IsShared = true,
                IsSharedForEveryone = false,
                FileType = DAO.Models.Base.FileType.DIRECTORY
            };

            await databaseContext.Directories.AddAsync(directory);

            await databaseContext.SaveChangesAsync();

            bool isSharedForEveryone = await shareService.IsSharedForEveryone(directory.ID);

            Assert.False(isSharedForEveryone);
        }

        [Fact]
        public async void Returns_False_when_FileIsNotSharedForEveryone()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();

            IShareService shareService = new ShareService(databaseContext, null);
            string ownerUsername = "admin@admin.com";
            ApplicationUser owner = await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ownerUsername);

            Assert.NotNull(owner);

            File file = new File
            {
                ParentDirectoryID = null,
                Name = "TestFile.txt",
                OwnerID = owner.Id,
                IsShared = true,
                IsSharedForEveryone = false,
                FileType = DAO.Models.Base.FileType.FILE
            };

            await databaseContext.Files.AddAsync(file);

            await databaseContext.SaveChangesAsync();

            bool isSharedForEveryone = await shareService.IsSharedForEveryone(file.ID);

            Assert.False(isSharedForEveryone);
        }
    }

    public class ShareServiceTest_ShareForEveryone
    {
        [Fact]
        public async void Returns_ShareEveryoneOut_when_FileSharedForEveryone()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();

            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new ShareEveryone_to_ShareEveryoneOut());
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            IShareService shareService = new ShareService(databaseContext, mapper);

            string ownerUsername = "admin@admin.com";
            ApplicationUser owner = await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ownerUsername);

            Assert.NotNull(owner);

            File file = new File
            {
                ParentDirectoryID = null,
                Name = "TestFile.txt",
                OwnerID = owner.Id,
                FileType = DAO.Models.Base.FileType.FILE
            };

            await databaseContext.Files.AddAsync(file);
            await databaseContext.SaveChangesAsync();

            ShareEveryoneOut shareEveryoneOut = await shareService.ShareForEveryone(file.ID, ownerUsername, null, null, null);

            Assert.NotNull(shareEveryoneOut);
            Assert.True(file.IsShared);
            Assert.True(file.IsSharedForEveryone);
        }

        [Fact]
        public async void Returns_Null_when_FileNotExist()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();

            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new ShareEveryone_to_ShareEveryoneOut());
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            IShareService shareService = new ShareService(databaseContext, mapper);

            string ownerUsername = "admin@admin.com";
            ApplicationUser owner = await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ownerUsername);

            Assert.NotNull(owner);

            ShareEveryoneOut shareEveryoneOut = await shareService.ShareForEveryone(Guid.NewGuid(), ownerUsername, null, null, null);

            Assert.Null(shareEveryoneOut);
        }

        [Fact]
        public async void Returns_Null_when_FileNotBelongsToLoggedUser()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();

            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new ShareEveryone_to_ShareEveryoneOut());
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            IShareService shareService = new ShareService(databaseContext, mapper);

            string ownerUsername = "admin@admin.com";
            ApplicationUser owner = await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ownerUsername);

            Assert.NotNull(owner);

            File file = new File
            {
                ParentDirectoryID = null,
                Name = "TestFile.txt",
                OwnerID = Guid.NewGuid().ToString(),
                FileType = DAO.Models.Base.FileType.FILE
            };

            await databaseContext.Files.AddAsync(file);
            await databaseContext.SaveChangesAsync();

            ShareEveryoneOut shareEveryoneOut = await shareService.ShareForEveryone(file.ID, ownerUsername, null, null, null);

            Assert.Null(shareEveryoneOut);
        }
    }

    public class ShareServiceTest_CancelSharingForEveryone
    {
        [Fact]
        public async void Returns_True_when_CanceledSharingFileForEveryone()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();

            IShareService shareService = new ShareService(databaseContext, null);

            string ownerUsername = "admin@admin.com";
            ApplicationUser owner = await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ownerUsername);

            Assert.NotNull(owner);

            File file = new File
            {
                ParentDirectoryID = null,
                Name = "TestFile.txt",
                OwnerID = owner.Id,
                IsShared = true,
                IsSharedForEveryone = true,
                FileType = DAO.Models.Base.FileType.FILE
            };

            await databaseContext.Files.AddAsync(file);
            ShareEveryone shareFile = new ShareEveryone
            {
                CreatedDateTime = DateTime.Now,
                FileID = file.ID,
                OwnerID = owner.Id,
                Token = "fil_token"
            };

            await databaseContext.ShareEveryones.AddAsync(shareFile);

            await databaseContext.SaveChangesAsync();

            bool isSharedForEveryone = await shareService.IsSharedForEveryone(file.ID);
            Assert.True(isSharedForEveryone);

            bool? canceled = await shareService.CancelSharingForEveryone(file.ID, ownerUsername);
            Assert.True(canceled);
            Assert.False(await databaseContext.ShareEveryones.AnyAsync(_ => _.FileID == file.ID));
        }

        [Fact]
        public async void Returns_False_when_FileNotBelongsToUser()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();

            IShareService shareService = new ShareService(databaseContext, null);

            string ownerUsername = "admin@admin.com";
            ApplicationUser owner = await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ownerUsername);

            Assert.NotNull(owner);

            File file = new File
            {
                ParentDirectoryID = null,
                Name = "TestFile.txt",
                OwnerID = owner.Id,
                IsShared = true,
                IsSharedForEveryone = true,
                FileType = DAO.Models.Base.FileType.FILE
            };

            await databaseContext.Files.AddAsync(file);
            ShareEveryone shareFile = new ShareEveryone
            {
                CreatedDateTime = DateTime.Now,
                FileID = file.ID,
                OwnerID = owner.Id,
                Token = "fil_token"
            };

            await databaseContext.ShareEveryones.AddAsync(shareFile);

            await databaseContext.SaveChangesAsync();

            bool isSharedForEveryone = await shareService.IsSharedForEveryone(file.ID);
            Assert.True(isSharedForEveryone);

            bool canceled = await shareService.CancelSharingForEveryone(file.ID, "user@user.com");
            Assert.False(canceled);
            Assert.True(await databaseContext.ShareEveryones.AnyAsync(_ => _.FileID == file.ID));
        }

        [Fact]
        public async void Returns_False_when_FileNotExists()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();

            IShareService shareService = new ShareService(databaseContext, null);

            string ownerUsername = "admin@admin.com";
            ApplicationUser owner = await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ownerUsername);

            Assert.NotNull(owner);

            bool canceled = await shareService.CancelSharingForEveryone(Guid.NewGuid(), "user@user.com");
            Assert.False(canceled);
        }
    }
}
