using AutoMapper;
using DataDrive.DAO.Context;
using DataDrive.DAO.Helpers.Communication;
using DataDrive.DAO.Models;
using DataDrive.DAO.Models.Base;
using DataDrive.Files.Models;
using DataDrive.Files.Models.In;
using DataDrive.Files.Models.Out;
using DataDrive.Files.Services;
using DataDrive.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.StaticFiles;
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
        public async void Returns_Status201Created_when_Created()
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

            StatusCode<DirectoryOut> result = await fileService.CreateDirectoryByUser(directoryPost, "admin@admin.com");
            var a = databaseContext.Users.ToList();
            Assert.NotNull(result);
            Assert.True(result.Code == StatusCodes.Status201Created);
            Assert.True(result.Body.ParentDirectoryID == directoryPost.ParentDirectoryID);
            Assert.True(result.Body.Name == directoryPost.Name);
            Assert.True(databaseContext.Directories.Any(_ => _.ID == result.Body.ID));
        }

        [Fact]
        public async void Returns_Status404NotFound_when_ParentDirectoryNotExistOrNotBelongsToUser()
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

            StatusCode<DirectoryOut> result = await fileService.CreateDirectoryByUser(directoryPost, "admin@admin.com");

            Assert.NotNull(result);
            Assert.True(result.Code == StatusCodes.Status404NotFound);
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
        public async void Returns_ParentDirectoryAndStatus200OK_whenSuccessDeletedFile()
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
                ResourceType = ResourceType.DIRECTORY,
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
                ResourceType = ResourceType.FILE,
                Name = "ToDelete.txt",
                OwnerID = userId,
                ParentDirectoryID = baseDirectory.ID,
                Path = pathToFile,
                CreatedDateTime = DateTime.Now
            };

            await databaseContext.Files.AddAsync(fileToDelete);
            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Files.Any(_ => _.ID == fileToDelete.ID));

            StatusCode<DirectoryOut> status = await fileService.DeleteByIdAndUser(fileToDelete.ID, "admin@admin.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status200OK);
            Assert.True(status.Body.ID == baseDirectory.ID);
            Assert.False(databaseContext.Files.Any(_ => _.ID == fileToDelete.ID));
            Assert.False(System.IO.File.Exists(pathToFile));
        }

        [Fact]
        public async void Returns_ParentDirectoryAndStatus200OK_whenSuccessDeletedDirectory()
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
                ResourceType = ResourceType.DIRECTORY,
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
                ResourceType = ResourceType.FILE,
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
                ResourceType = ResourceType.DIRECTORY,
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
                ResourceType = ResourceType.FILE,
                Name = "file2.txt",
                OwnerID = userId,
                ParentDirectoryID = directoryInDirectoryToDelete.ID,
                Path = pathToFile2,
                CreatedDateTime = DateTime.Now
            };

            await databaseContext.Files.AddAsync(file2InDirectoryToDelete);
            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Files.Any(_ => _.ID == file2InDirectoryToDelete.ID));

            StatusCode<DirectoryOut> status = await fileService.DeleteByIdAndUser(directoryToDelete.ID, "admin@admin.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status200OK);
            Assert.True(status.Body.ID == null);
            Assert.False(databaseContext.Files.Any(_ => _.ID == file1InDirectoryToDelete.ID));
            Assert.False(System.IO.File.Exists(pathToFile));

            Assert.False(databaseContext.Files.Any(_ => _.ID == file2InDirectoryToDelete.ID));
            Assert.False(System.IO.File.Exists(pathToFile2));

            Assert.False(databaseContext.Files.Any(_ => _.ID == directoryToDelete.ID));
            Assert.False(databaseContext.Files.Any(_ => _.ID == directoryInDirectoryToDelete.ID));
        }

        [Fact]
        public async void Returns_Status404NotFound_whenFileNotExist()
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
                ResourceType = DAO.Models.Base.ResourceType.DIRECTORY,
                Name = "TestDirectory",
                OwnerID = userId
            };

            await databaseContext.Directories.AddAsync(baseDirectory);
            await databaseContext.SaveChangesAsync();

            StatusCode<DirectoryOut> status = await fileService.DeleteByIdAndUser(Guid.NewGuid(), "admin@admin.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status404NotFound);
        }

        [Fact]
        public async void Returns_Status404NotFound_whenFileNotBelongsToUser()
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
                ResourceType = ResourceType.DIRECTORY,
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
                ResourceType = ResourceType.FILE,
                Name = "ToDelete.txt",
                OwnerID = userId,
                ParentDirectoryID = baseDirectory.ID,
                Path = pathToFile,
                CreatedDateTime = DateTime.Now
            };

            await databaseContext.Files.AddAsync(fileToDelete);
            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Files.Any(_ => _.ID == fileToDelete.ID));

            StatusCode<DirectoryOut> status = await fileService.DeleteByIdAndUser(fileToDelete.ID, "user@user.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status404NotFound);
            Assert.True(databaseContext.Files.Any(_ => _.ID == fileToDelete.ID));
            Assert.True(System.IO.File.Exists(pathToFile));
        }
    }

    public class FileServiceTest_DownloadByIdAndUser
    {
        private readonly string _pathToUpload;

        public FileServiceTest_DownloadByIdAndUser()
        {
            _pathToUpload = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestUpload");

            if (!System.IO.Directory.Exists(_pathToUpload))
            {
                System.IO.Directory.CreateDirectory(_pathToUpload);
            }
        }

        [Fact]
        public async void Returns_DownloadFileInfoAndStatus200OK_when_SuccessDownload()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            IFileService fileService = new FileService(databaseContext, null);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            string pathToFile = System.IO.Path.Combine(_pathToUpload, Guid.NewGuid().ToString());
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes("Test file"));
            System.IO.FileStream fileStream = new System.IO.FileStream(pathToFile, System.IO.FileMode.Create);
            await memoryStream.CopyToAsync(fileStream);
            fileStream.Close();

            Assert.True(System.IO.File.Exists(pathToFile));

            File fileToDownload = new File
            {
                ResourceType = ResourceType.FILE,
                Name = "ToDownload.txt",
                OwnerID = userId,
                ParentDirectoryID = null,
                Path = pathToFile,
                CreatedDateTime = DateTime.Now
            };

            await databaseContext.Files.AddAsync(fileToDownload);
            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Files.Any(_ => _.ID == fileToDownload.ID));

            //Get MIME type
            FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileToDownload.Name, out string contentType))
            {
                contentType = "application/octet-stream";
            }

            byte[] fileContent = System.IO.File.ReadAllBytes(pathToFile);

            StatusCode<DownloadFileInfo> status = await fileService.DownloadByIdAndUser(fileToDownload.ID, "admin@admin.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status200OK);
            Assert.Equal(fileToDownload.Name, status.Body.FileName);
            Assert.Equal(fileContent, status.Body.FileContent);
            Assert.Equal(contentType, status.Body.ContentType);
        }

        [Fact]
        public async void Returns_Status404NotFound_when_FileNotBelongsToUser()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            IFileService fileService = new FileService(databaseContext, null);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            string pathToFile = System.IO.Path.Combine(_pathToUpload, Guid.NewGuid().ToString());
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes("Test file"));
            System.IO.FileStream fileStream = new System.IO.FileStream(pathToFile, System.IO.FileMode.Create);
            await memoryStream.CopyToAsync(fileStream);
            fileStream.Close();

            Assert.True(System.IO.File.Exists(pathToFile));

            File fileToDownload = new File
            {
                ResourceType = ResourceType.FILE,
                Name = "ToDownload.txt",
                OwnerID = userId,
                ParentDirectoryID = null,
                Path = pathToFile,
                CreatedDateTime = DateTime.Now
            };

            await databaseContext.Files.AddAsync(fileToDownload);
            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Files.Any(_ => _.ID == fileToDownload.ID));

            StatusCode<DownloadFileInfo> status = await fileService.DownloadByIdAndUser(fileToDownload.ID, "user@user.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status404NotFound);
        }

        [Fact]
        public async void Returns_Status404NotFound_when_FileNotExist()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            IFileService fileService = new FileService(databaseContext, null);

            StatusCode<DownloadFileInfo> status = await fileService.DownloadByIdAndUser(Guid.NewGuid(), "admin@admin.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status404NotFound);
        }
    }

    public class FileServiceTest_GetByIdAndUser
    {
        [Fact]
        public async void Returns_FileFileOutAndStatus200OK_when_Success()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration config = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new FileAbstract_to_FileOut());
            });
            IMapper mapper = config.CreateMapper();

            IFileService fileService = new FileService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            File file = new File
            {
                CreatedDateTime = DateTime.Now,
                ResourceType = ResourceType.FILE,
                Name = "TestFile.pdf",
                OwnerID = userId
            };

            await databaseContext.Files.AddAsync(file);
            await databaseContext.SaveChangesAsync();

            StatusCode<FileOut> status = await fileService.GetByIdAndUser(file.ID, "admin@admin.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status200OK);
            Assert.NotNull(status.Body);
            Assert.Equal(file.ID, status.Body.ID);
            Assert.Equal(file.ResourceType, status.Body.ResourceType);
            Assert.Equal(file.CreatedDateTime, status.Body.CreatedDateTime);
            Assert.Equal(file.Name, status.Body.Name);
            Assert.Equal(file.ParentDirectoryID, status.Body.ParentDirectoryID);
        }

        [Fact]
        public async void Returns_DirectoryFileOutAndStatus200OK_when_Success()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration config = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new FileAbstract_to_FileOut());
            });
            IMapper mapper = config.CreateMapper();

            IFileService fileService = new FileService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            Directory directoryToCheck = new Directory
            {
                CreatedDateTime = DateTime.Now,
                ResourceType = ResourceType.DIRECTORY,
                Name = "Directory",
                OwnerID = userId
            };

            await databaseContext.Directories.AddAsync(directoryToCheck);
            await databaseContext.SaveChangesAsync();

            File file = new File
            {
                CreatedDateTime = DateTime.Now,
                ResourceType = ResourceType.FILE,
                Name = "TestFile.pdf",
                OwnerID = userId,
                ParentDirectoryID = directoryToCheck.ID
            };

            await databaseContext.Files.AddAsync(file);
            await databaseContext.SaveChangesAsync();

            StatusCode<FileOut> status = await fileService.GetByIdAndUser(directoryToCheck.ID, "admin@admin.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status200OK);
            Assert.NotNull(status.Body);
            Assert.Equal(directoryToCheck.ID, status.Body.ID);
            Assert.Equal(directoryToCheck.ResourceType, status.Body.ResourceType);
            Assert.Equal(directoryToCheck.CreatedDateTime, status.Body.CreatedDateTime);
            Assert.Equal(directoryToCheck.Name, status.Body.Name);
            Assert.Equal(directoryToCheck.ParentDirectoryID, status.Body.ParentDirectoryID);
        }

        [Fact]
        public async void Returns_Status404NotFound_when_FileNotBelongsToUser()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration config = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new FileAbstract_to_FileOut());
            });
            IMapper mapper = config.CreateMapper();

            IFileService fileService = new FileService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            File file = new File
            {
                CreatedDateTime = DateTime.Now,
                ResourceType = ResourceType.FILE,
                Name = "TestFile.pdf",
                OwnerID = userId
            };

            await databaseContext.Files.AddAsync(file);
            await databaseContext.SaveChangesAsync();

            StatusCode<FileOut> status = await fileService.GetByIdAndUser(file.ID, "user@user.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status404NotFound);
        }

        [Fact]
        public async void Returns_Status404NotFound_when_FileNotExist()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration config = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new FileAbstract_to_FileOut());
            });
            IMapper mapper = config.CreateMapper();

            IFileService fileService = new FileService(databaseContext, mapper);

            StatusCode<FileOut> status = await fileService.GetByIdAndUser(Guid.NewGuid(), "admin@admin.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status404NotFound);
        }
    }

    public class FileServiceTest_GetDirectoryByIdAndUser
    {
        [Fact]
        public async void Returns_DirectoryWithFileListAndStatus200OK_when_Success()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration config = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Directory_to_DirectoryOut());
            });
            IMapper mapper = config.CreateMapper();

            IFileService fileSerivce = new FileService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            Directory rootDirectory = new Directory
            {
                CreatedDateTime = DateTime.Now,
                ResourceType = ResourceType.DIRECTORY,
                Name = "root",
                OwnerID = userId
            };

            await databaseContext.Directories.AddAsync(rootDirectory);

            Directory directoryToCheck = new Directory
            {
                CreatedDateTime = DateTime.Now,
                ResourceType = ResourceType.DIRECTORY,
                Name = "DirectoryToCheck",
                OwnerID = userId,
                ParentDirectoryID = rootDirectory.ID
            };

            await databaseContext.Directories.AddAsync(directoryToCheck);

            File file1 = new File
            {
                CreatedDateTime = DateTime.Now,
                ResourceType = ResourceType.FILE,
                Name = "File1.exe",
                OwnerID = userId,
                ParentDirectoryID = directoryToCheck.ID
            };

            File file2 = new File
            {
                CreatedDateTime = DateTime.Now,
                ResourceType = ResourceType.FILE,
                Name = "File2.rar",
                OwnerID = userId,
                ParentDirectoryID = directoryToCheck.ID
            };

            await databaseContext.Files.AddRangeAsync(file1, file2);

            Directory directory1 = new Directory
            {
                CreatedDateTime = DateTime.Now,
                ResourceType = ResourceType.DIRECTORY,
                Name = "Directory1.d",
                OwnerID = userId,
                ParentDirectoryID = directoryToCheck.ID
            };

            await databaseContext.Directories.AddAsync(directory1);

            await databaseContext.SaveChangesAsync();

            StatusCode<DirectoryOut> status = await fileSerivce.GetDirectoryByIdAndUser(directoryToCheck.ID, "admin@admin.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status200OK);
            Assert.NotNull(status.Body);
            Assert.Equal(directoryToCheck.CreatedDateTime, status.Body.CreatedDateTime);
            Assert.Equal(directoryToCheck.Files.Count, status.Body.Files.Count);
            Assert.Equal(directoryToCheck.ResourceType, status.Body.ResourceType);
            Assert.Equal(directoryToCheck.ID, status.Body.ID);
            Assert.Equal(directoryToCheck.Name, status.Body.Name);
            Assert.Equal(directoryToCheck.ParentDirectoryID, status.Body.ParentDirectoryID);
            Assert.Equal(directoryToCheck.ParentDirectory.Name, status.Body.ParentDirectoryName);
        }

        [Fact]
        public async void Returns_DirectoryWithFileListAndStatus200OK_when_ParentDirectoryIsNull()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration config = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Directory_to_DirectoryOut());
            });
            IMapper mapper = config.CreateMapper();

            IFileService fileSerivce = new FileService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            File file1 = new File
            {
                CreatedDateTime = DateTime.Now,
                ResourceType = ResourceType.FILE,
                Name = "File1.exe",
                OwnerID = userId
            };

            File file2 = new File
            {
                CreatedDateTime = DateTime.Now,
                ResourceType = ResourceType.FILE,
                Name = "File2.rar",
                OwnerID = userId
            };

            await databaseContext.Files.AddRangeAsync(file1, file2);

            Directory directory1 = new Directory
            {
                CreatedDateTime = DateTime.Now,
                ResourceType = ResourceType.DIRECTORY,
                Name = "Directory1.d",
                OwnerID = userId
            };

            await databaseContext.Directories.AddAsync(directory1);

            await databaseContext.SaveChangesAsync();

            StatusCode<DirectoryOut> status = await fileSerivce.GetDirectoryByIdAndUser(null, "admin@admin.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status200OK);
            Assert.NotNull(status.Body);

            List<FileAbstract> filesFromRoot = await databaseContext.FileAbstracts
                .Where(_ => _.ParentDirectoryID == null && _.OwnerID == userId)
                .ToListAsync();

            Assert.Equal(filesFromRoot.Count, status.Body.Files.Count);
            Assert.Equal(ResourceType.DIRECTORY, status.Body.ResourceType);
            Assert.Null(status.Body.ID);
            Assert.Null(status.Body.ParentDirectoryID);
        }

        [Fact]
        public async void Returns_Status404NotFound_when_DirectoryNotExist()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration config = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Directory_to_DirectoryOut());
            });
            IMapper mapper = config.CreateMapper();

            IFileService fileSerivce = new FileService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            StatusCode<DirectoryOut> status = await fileSerivce.GetDirectoryByIdAndUser(Guid.NewGuid(), "admin@admin.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status404NotFound);
        }

        [Fact]
        public async void Returns_Status404NotFound_when_DirectoryNotBelongsToUser()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration config = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Directory_to_DirectoryOut());
            });
            IMapper mapper = config.CreateMapper();

            IFileService fileSerivce = new FileService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            Directory rootDirectory = new Directory
            {
                CreatedDateTime = DateTime.Now,
                ResourceType = ResourceType.DIRECTORY,
                Name = "root",
                OwnerID = userId
            };

            await databaseContext.Directories.AddAsync(rootDirectory);

            Directory directoryToCheck = new Directory
            {
                CreatedDateTime = DateTime.Now,
                ResourceType = ResourceType.DIRECTORY,
                Name = "DirectoryToCheck",
                OwnerID = userId,
                ParentDirectoryID = rootDirectory.ID
            };

            await databaseContext.Directories.AddAsync(directoryToCheck);

            File file1 = new File
            {
                CreatedDateTime = DateTime.Now,
                ResourceType = ResourceType.FILE,
                Name = "File1.exe",
                OwnerID = userId,
                ParentDirectoryID = directoryToCheck.ID
            };

            File file2 = new File
            {
                CreatedDateTime = DateTime.Now,
                ResourceType = ResourceType.FILE,
                Name = "File2.rar",
                OwnerID = userId,
                ParentDirectoryID = directoryToCheck.ID
            };

            await databaseContext.Files.AddRangeAsync(file1, file2);

            Directory directory1 = new Directory
            {
                CreatedDateTime = DateTime.Now,
                ResourceType = ResourceType.DIRECTORY,
                Name = "Directory1.d",
                OwnerID = userId,
                ParentDirectoryID = directoryToCheck.ID
            };

            await databaseContext.Directories.AddAsync(directory1);

            await databaseContext.SaveChangesAsync();

            StatusCode<DirectoryOut> status = await fileSerivce.GetDirectoryByIdAndUser(directoryToCheck.ID, "user@user.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status404NotFound);
        }

        [Fact]
        public async void Returns_Status404NotFound_when_DirectoryIsNotADirectory()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration config = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Directory_to_DirectoryOut());
            });
            IMapper mapper = config.CreateMapper();

            IFileService fileSerivce = new FileService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            File fileToCheck = new File
            {
                CreatedDateTime = DateTime.Now,
                ResourceType = ResourceType.FILE,
                Name = "DirectoryToCheck",
                OwnerID = userId
            };

            await databaseContext.Files.AddAsync(fileToCheck);

            await databaseContext.SaveChangesAsync();

            StatusCode<DirectoryOut> status = await fileSerivce.GetDirectoryByIdAndUser(fileToCheck.ID, "admin@admin.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status404NotFound);
        }
    }

    public class FileServiceTest_PatchByIdAndFilePatchAndUser
    {
        [Fact]
        public async void Returns_PatchedFileOutAndStatus200OK_when_ChangedParentDirectory()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration configuration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new FileAbstract_to_FileOut());
                conf.AddProfile(new JsonPatchDocument_Mapper());
            });
            IMapper mapper = configuration.CreateMapper();

            IFileService fileService = new FileService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            Directory directory = new Directory
            {
                ResourceType = ResourceType.DIRECTORY,
                Name = "Directory",
                OwnerID = userId,
            };

            await databaseContext.Directories.AddAsync(directory);

            File fileToMove = new File
            {
                ResourceType = ResourceType.FILE,
                Name = "File.txt",
                OwnerID = userId
            };

            await databaseContext.Files.AddAsync(fileToMove);
            await databaseContext.SaveChangesAsync();

            JsonPatchDocument<FilePatch> jsonPatchDocument = new JsonPatchDocument<FilePatch>();
            jsonPatchDocument.Add(_ => _.ParentDirectoryID, directory.ID);

            StatusCode<FileOut> status = await fileService.PatchByIdAndFilePatchAndUser(fileToMove.ID, jsonPatchDocument, "admin@admin.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status200OK);
            Assert.NotNull(status.Body);
            Assert.Equal(directory.ID, status.Body.ParentDirectoryID);
            Assert.Equal(fileToMove.Name, status.Body.Name);
        }

        [Fact]
        public async void Returns_PatchedFileOutAndStatus200OK_when_ChangedName()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration configuration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new FileAbstract_to_FileOut());
                conf.AddProfile(new JsonPatchDocument_Mapper());
            });
            IMapper mapper = configuration.CreateMapper();

            IFileService fileService = new FileService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            File fileToChange = new File
            {
                ResourceType = ResourceType.FILE,
                Name = "File.txt",
                OwnerID = userId
            };

            await databaseContext.Files.AddAsync(fileToChange);
            await databaseContext.SaveChangesAsync();

            string newFileName = "newName.pdf";

            JsonPatchDocument<FilePatch> jsonPatchDocument = new JsonPatchDocument<FilePatch>();
            jsonPatchDocument.Add(_ => _.Name, newFileName);

            StatusCode<FileOut> status = await fileService.PatchByIdAndFilePatchAndUser(fileToChange.ID, jsonPatchDocument, "admin@admin.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status200OK);
            Assert.NotNull(status.Body);
            Assert.Equal(fileToChange.ParentDirectoryID, status.Body.ParentDirectoryID);
            Assert.Equal(newFileName, status.Body.Name);
        }

        [Fact]
        public async void Returns_PatchedFileOutAndStatus200OK_when_ChangedNameAndParentDirectory()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration configuration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new FileAbstract_to_FileOut());
                conf.AddProfile(new JsonPatchDocument_Mapper());
            });
            IMapper mapper = configuration.CreateMapper();

            IFileService fileService = new FileService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            Directory directory = new Directory
            {
                ResourceType = ResourceType.DIRECTORY,
                Name = "Directory",
                OwnerID = userId,
            };

            await databaseContext.Directories.AddAsync(directory);

            File fileToChange = new File
            {
                ResourceType = ResourceType.FILE,
                Name = "File.txt",
                OwnerID = userId
            };

            await databaseContext.Files.AddAsync(fileToChange);
            await databaseContext.SaveChangesAsync();

            string newFileName = "newName.pdf";

            JsonPatchDocument<FilePatch> jsonPatchDocument = new JsonPatchDocument<FilePatch>();
            jsonPatchDocument.Add(_ => _.ParentDirectoryID, directory.ID);
            jsonPatchDocument.Add(_ => _.Name, newFileName);

            StatusCode<FileOut> status = await fileService.PatchByIdAndFilePatchAndUser(fileToChange.ID, jsonPatchDocument, "admin@admin.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status200OK);
            Assert.NotNull(status.Body);
            Assert.Equal(directory.ID, status.Body.ParentDirectoryID);
            Assert.Equal(newFileName, status.Body.Name);
        }

        [Fact]
        public async void Returns_Status404NotFound_when_FileNotBelongsToUser()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration configuration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new FileAbstract_to_FileOut());
            });
            IMapper mapper = configuration.CreateMapper();

            IFileService fileService = new FileService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            File fileToChange = new File
            {
                ResourceType = ResourceType.FILE,
                Name = "File.txt",
                OwnerID = userId
            };

            await databaseContext.Files.AddAsync(fileToChange);
            await databaseContext.SaveChangesAsync();

            JsonPatchDocument<FilePatch> jsonPatchDocument = new JsonPatchDocument<FilePatch>();
            jsonPatchDocument.Add(_ => _.Name, "newName");

            StatusCode<FileOut> status = await fileService.PatchByIdAndFilePatchAndUser(fileToChange.ID, jsonPatchDocument, "user@user.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status404NotFound);
        }

        [Fact]
        public async void Returns_Status404NotFound_when_FileNotExist()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration configuration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new FileAbstract_to_FileOut());
            });
            IMapper mapper = configuration.CreateMapper();

            IFileService fileService = new FileService(databaseContext, mapper);

            JsonPatchDocument<FilePatch> jsonPatchDocument = new JsonPatchDocument<FilePatch>();
            jsonPatchDocument.Add(_ => _.Name, "newName");

            StatusCode<FileOut> status = await fileService.PatchByIdAndFilePatchAndUser(Guid.NewGuid(), jsonPatchDocument, "admin@admin.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status404NotFound);
        }
    }

    public class FileServiceTest_PostByUser
    {
        [Fact]
        public async void Returns_ListOfFileUploadResultAndStatus200OK_when_UploadedMultipleFile()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();

            IFileService fileService = new FileService(databaseContext, null);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            Directory directory = new Directory
            {
                ResourceType = ResourceType.DIRECTORY,
                OwnerID = userId,
                Name = "Directory"
            };

            await databaseContext.Directories.AddAsync(directory);
            await databaseContext.SaveChangesAsync();

            FilePost filePost = new FilePost
            {
                ParentDirectoryID = directory.ID,
                Files = new FormFileCollection
                {
                    new FormFile(
                        new System.IO.MemoryStream(Encoding.UTF8.GetBytes("text")),
                        0, 4, "Data", "file1.txt"),
                    new FormFile(
                        new System.IO.MemoryStream(Encoding.UTF8.GetBytes("Loooong text")),
                        0, 12, "Data", "file2.txt")
                }
            };

            StatusCode<List<FileUploadResult>> status = await fileService.PostByUser(filePost, "admin@admin.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status200OK);
            Assert.NotNull(status.Body);
            Assert.Equal(filePost.Files.Count(), status.Body.Count());

            Assert.True(databaseContext.Files
                .Any(_ => _.ParentDirectoryID == directory.ID && _.Name == "file1.txt"));
            Assert.True(databaseContext.Files
                .Any(_ => _.ParentDirectoryID == directory.ID && _.Name == "file2.txt"));
        }

        [Fact]
        public async void Returns_ListOfFileUploadResultAndStatus200OK_when_UploadedSingleFile()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();

            IFileService fileService = new FileService(databaseContext, null);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            Directory directory = new Directory
            {
                ResourceType = ResourceType.DIRECTORY,
                OwnerID = userId,
                Name = "Directory"
            };

            await databaseContext.Directories.AddAsync(directory);
            await databaseContext.SaveChangesAsync();

            FilePost filePost = new FilePost
            {
                ParentDirectoryID = directory.ID,
                Files = new FormFileCollection
                {
                    new FormFile(
                        new System.IO.MemoryStream(Encoding.UTF8.GetBytes("text")),
                        0, 4, "Data", "file1.txt")
                }
            };

            StatusCode<List<FileUploadResult>> status = await fileService.PostByUser(filePost, "admin@admin.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status200OK);
            Assert.NotNull(status.Body);
            Assert.Equal(filePost.Files.Count(), status.Body.Count());

            Assert.True(databaseContext.Files
                .Any(_ => _.ParentDirectoryID == directory.ID && _.Name == "file1.txt"));
        }

        [Fact]
        public async void Returns_ListOfFileUploadResultAndStatus200OK_when_ParentDirectoryIsNull()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();

            IFileService fileService = new FileService(databaseContext, null);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            FilePost filePost = new FilePost
            {
                Files = new FormFileCollection
                {
                    new FormFile(
                        new System.IO.MemoryStream(Encoding.UTF8.GetBytes("text")),
                        0, 4, "Data", "file1.txt")
                }
            };

            StatusCode<List<FileUploadResult>> status = await fileService.PostByUser(filePost, "admin@admin.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status200OK);
            Assert.NotNull(status.Body);
            Assert.Equal(filePost.Files.Count(), status.Body.Count());

            Assert.True(databaseContext.Files
                .Any(_ => _.ParentDirectoryID == null && _.Name == "file1.txt"));
        }

        [Fact]
        public async void Returns_Status404NotFound_when_ParentDirectoryNotExist()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();

            IFileService fileService = new FileService(databaseContext, null);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            FilePost filePost = new FilePost
            {
                ParentDirectoryID = Guid.NewGuid(),
                Files = new FormFileCollection
                {
                    new FormFile(
                        new System.IO.MemoryStream(Encoding.UTF8.GetBytes("text")),
                        0, 4, "Data", "file1.txt")
                }
            };

            StatusCode<List<FileUploadResult>> status = await fileService.PostByUser(filePost, "admin@admin.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status404NotFound);
        }

        [Fact]
        public async void Returns_Status404NotFound_when_ParentDirectoryNotBelongsToUser()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();

            IFileService fileService = new FileService(databaseContext, null);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            Directory directory = new Directory
            {
                ResourceType = ResourceType.DIRECTORY,
                OwnerID = userId,
                Name = "Directory"
            };

            await databaseContext.Directories.AddAsync(directory);

            FilePost filePost = new FilePost
            {
                ParentDirectoryID = directory.ID,
                Files = new FormFileCollection
                {
                    new FormFile(
                        new System.IO.MemoryStream(Encoding.UTF8.GetBytes("text")),
                        0, 4, "Data", "file1.txt")
                }
            };

            StatusCode<List<FileUploadResult>> status = await fileService.PostByUser(filePost, "user@user.com");

            Assert.NotNull(status);
            Assert.True(status.Code == StatusCodes.Status404NotFound);
        }
    }
}
