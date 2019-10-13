using AutoMapper;
using DataDrive.DAO.Context;
using DataDrive.Files.Models.In;
using DataDrive.Files.Models.Out;
using DataDrive.Files.Services;
using DataDrive.Tests.Helpers;
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
}
