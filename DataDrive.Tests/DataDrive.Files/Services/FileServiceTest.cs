using AutoMapper;
using DataDrive.DAO.Context;
using DataDrive.DAO.Models;
using DataDrive.DAO.Models.Base;
using DataDrive.Files.Models.In;
using DataDrive.Files.Models.Out;
using DataDrive.Files.Services;
using DataDrive.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
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
        public async void Returns_TupleFileNameContentArrayContentType()
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
                FileType = DAO.Models.Base.FileType.FILE,
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
            //return new Tuple<string, byte[], string>(fileToDownload.Name, fileContent, contentType);

            Tuple<string, byte[], string> result = await fileService.DownloadByIdAndUser(fileToDownload.ID, "admin@admin.com");

            Assert.Equal(fileToDownload.Name, result.Item1);
            Assert.Equal(fileContent, result.Item2);
            Assert.Equal(contentType, result.Item3);
        }

        [Fact]
        public async void Returns_Null_when_FileNotBelongsToUser()
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
                FileType = DAO.Models.Base.FileType.FILE,
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
            //return new Tuple<string, byte[], string>(fileToDownload.Name, fileContent, contentType);

            Tuple<string, byte[], string> result = await fileService.DownloadByIdAndUser(fileToDownload.ID, "user@user.com");

            Assert.Null(result);
        }

        [Fact]
        public async void Returns_Null_when_FileNotExist()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            IFileService fileService = new FileService(databaseContext, null);

            Tuple<string, byte[], string> result = await fileService.DownloadByIdAndUser(Guid.NewGuid(), "admin@admin.com");

            Assert.Null(result);
        }
    }

    public class FileServiceTest_GetByIdAndUser
    {
        [Fact]
        public async void Returns_FileInfo_when_Success()
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
                FileType = DAO.Models.Base.FileType.FILE,
                Name = "TestFile.pdf",
                OwnerID = userId
            };

            await databaseContext.Files.AddAsync(file);
            await databaseContext.SaveChangesAsync();

            FileOut result = await fileService.GetByIdAndUser(file.ID, "admin@admin.com");

            Assert.NotNull(result);
            Assert.Equal(file.ID, result.ID);
            Assert.Equal(file.FileType, result.FileType);
            Assert.Equal(file.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(file.Name, result.Name);
            Assert.Equal(file.ParentDirectoryID, result.ParentDirectoryID);
        }

        [Fact]
        public async void Returns_DirectoryInfo_when_Success()
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
                FileType = DAO.Models.Base.FileType.DIRECTORY,
                Name = "Directory",
                OwnerID = userId
            };

            await databaseContext.Directories.AddAsync(directoryToCheck);
            await databaseContext.SaveChangesAsync();

            File file = new File
            {
                CreatedDateTime = DateTime.Now,
                FileType = DAO.Models.Base.FileType.FILE,
                Name = "TestFile.pdf",
                OwnerID = userId,
                ParentDirectoryID = directoryToCheck.ID
            };

            await databaseContext.Files.AddAsync(file);
            await databaseContext.SaveChangesAsync();

            FileOut result = await fileService.GetByIdAndUser(directoryToCheck.ID, "admin@admin.com");

            Assert.NotNull(result);
            Assert.Equal(directoryToCheck.ID, result.ID);
            Assert.Equal(directoryToCheck.FileType, result.FileType);
            Assert.Equal(directoryToCheck.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(directoryToCheck.Name, result.Name);
            Assert.Equal(directoryToCheck.ParentDirectoryID, result.ParentDirectoryID);
        }

        [Fact]
        public async void Returns_Null_when_FileNotBelongsToUser()
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
                FileType = DAO.Models.Base.FileType.FILE,
                Name = "TestFile.pdf",
                OwnerID = userId
            };

            await databaseContext.Files.AddAsync(file);
            await databaseContext.SaveChangesAsync();

            FileOut result = await fileService.GetByIdAndUser(file.ID, "user@user.com");

            Assert.Null(result);
        }

        [Fact]
        public async void Returns_Null_when_FileNotExist()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration config = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new FileAbstract_to_FileOut());
            });
            IMapper mapper = config.CreateMapper();

            IFileService fileService = new FileService(databaseContext, mapper);

            FileOut result = await fileService.GetByIdAndUser(Guid.NewGuid(), "admin@admin.com");

            Assert.Null(result);
        }
    }

    public class FileServiceTest_GetDirectoryByIdAndUser
    {
        [Fact]
        public async void Returns_DirectoryWithFileList_when_Success()
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
                FileType = DAO.Models.Base.FileType.DIRECTORY,
                Name = "root",
                OwnerID = userId
            };

            await databaseContext.Directories.AddAsync(rootDirectory);

            Directory directoryToCheck = new Directory
            {
                CreatedDateTime = DateTime.Now,
                FileType = DAO.Models.Base.FileType.DIRECTORY,
                Name = "DirectoryToCheck",
                OwnerID = userId,
                ParentDirectoryID = rootDirectory.ID
            };

            await databaseContext.Directories.AddAsync(directoryToCheck);

            File file1 = new File
            {
                CreatedDateTime = DateTime.Now,
                FileType = DAO.Models.Base.FileType.FILE,
                Name = "File1.exe",
                OwnerID = userId,
                ParentDirectoryID = directoryToCheck.ID
            };

            File file2 = new File
            {
                CreatedDateTime = DateTime.Now,
                FileType = DAO.Models.Base.FileType.FILE,
                Name = "File2.rar",
                OwnerID = userId,
                ParentDirectoryID = directoryToCheck.ID
            };

            await databaseContext.Files.AddRangeAsync(file1, file2);

            Directory directory1 = new Directory
            {
                CreatedDateTime = DateTime.Now,
                FileType = DAO.Models.Base.FileType.DIRECTORY,
                Name = "Directory1.d",
                OwnerID = userId,
                ParentDirectoryID = directoryToCheck.ID
            };

            await databaseContext.Directories.AddAsync(directory1);

            await databaseContext.SaveChangesAsync();

            DirectoryOut result = await fileSerivce.GetDirectoryByIdAndUser(directoryToCheck.ID, "admin@admin.com");

            Assert.NotNull(result);
            Assert.Equal(directoryToCheck.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(directoryToCheck.Files.Count, result.Files.Count);
            Assert.Equal(directoryToCheck.FileType, result.FileType);
            Assert.Equal(directoryToCheck.ID, result.ID);
            Assert.Equal(directoryToCheck.Name, result.Name);
            Assert.Equal(directoryToCheck.ParentDirectoryID, result.ParentDirectoryID);
            Assert.Equal(directoryToCheck.ParentDirectory.Name, result.ParentDirectoryName);
        }

        [Fact]
        public async void Returns_DirectoryWithFileList_when_ParentDirectoryIsNull()
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
                FileType = DAO.Models.Base.FileType.FILE,
                Name = "File1.exe",
                OwnerID = userId
            };

            File file2 = new File
            {
                CreatedDateTime = DateTime.Now,
                FileType = DAO.Models.Base.FileType.FILE,
                Name = "File2.rar",
                OwnerID = userId
            };

            await databaseContext.Files.AddRangeAsync(file1, file2);

            Directory directory1 = new Directory
            {
                CreatedDateTime = DateTime.Now,
                FileType = DAO.Models.Base.FileType.DIRECTORY,
                Name = "Directory1.d",
                OwnerID = userId
            };

            await databaseContext.Directories.AddAsync(directory1);

            await databaseContext.SaveChangesAsync();

            DirectoryOut result = await fileSerivce.GetDirectoryByIdAndUser(null, "admin@admin.com");

            Assert.NotNull(result);

            List<FileAbstract> filesFromRoot = await databaseContext.FileAbstracts
                .Where(_ => _.ParentDirectoryID == null && _.OwnerID == userId)
                .ToListAsync();

            Assert.Equal(filesFromRoot.Count, result.Files.Count);
            Assert.Equal(FileType.DIRECTORY, result.FileType);
            Assert.Null(result.ID);
            Assert.Null(result.ParentDirectoryID);
        }

        [Fact]
        public async void Returns_Null_when_DirectoryNotExist()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration config = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Directory_to_DirectoryOut());
            });
            IMapper mapper = config.CreateMapper();

            IFileService fileSerivce = new FileService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            DirectoryOut result = await fileSerivce.GetDirectoryByIdAndUser(Guid.NewGuid(), "admin@admin.com");

            Assert.Null(result);
        }

        [Fact]
        public async void Returns_Null_when_DirectoryNotBelongsToUser()
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
                FileType = DAO.Models.Base.FileType.DIRECTORY,
                Name = "root",
                OwnerID = userId
            };

            await databaseContext.Directories.AddAsync(rootDirectory);

            Directory directoryToCheck = new Directory
            {
                CreatedDateTime = DateTime.Now,
                FileType = DAO.Models.Base.FileType.DIRECTORY,
                Name = "DirectoryToCheck",
                OwnerID = userId,
                ParentDirectoryID = rootDirectory.ID
            };

            await databaseContext.Directories.AddAsync(directoryToCheck);

            File file1 = new File
            {
                CreatedDateTime = DateTime.Now,
                FileType = DAO.Models.Base.FileType.FILE,
                Name = "File1.exe",
                OwnerID = userId,
                ParentDirectoryID = directoryToCheck.ID
            };

            File file2 = new File
            {
                CreatedDateTime = DateTime.Now,
                FileType = DAO.Models.Base.FileType.FILE,
                Name = "File2.rar",
                OwnerID = userId,
                ParentDirectoryID = directoryToCheck.ID
            };

            await databaseContext.Files.AddRangeAsync(file1, file2);

            Directory directory1 = new Directory
            {
                CreatedDateTime = DateTime.Now,
                FileType = DAO.Models.Base.FileType.DIRECTORY,
                Name = "Directory1.d",
                OwnerID = userId,
                ParentDirectoryID = directoryToCheck.ID
            };

            await databaseContext.Directories.AddAsync(directory1);

            await databaseContext.SaveChangesAsync();

            DirectoryOut result = await fileSerivce.GetDirectoryByIdAndUser(directoryToCheck.ID, "user@user.com");

            Assert.Null(result);
        }

        [Fact]
        public async void Returns_Null_when_DirectoryIsNotADirectory()
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
                FileType = DAO.Models.Base.FileType.FILE,
                Name = "DirectoryToCheck",
                OwnerID = userId
            };

            await databaseContext.Files.AddAsync(fileToCheck);

            await databaseContext.SaveChangesAsync();

            DirectoryOut result = await fileSerivce.GetDirectoryByIdAndUser(fileToCheck.ID, "admin@admin.com");

            Assert.Null(result);
        }
    }

    public class FileServiceTest_PatchByIdAndFilePatchAndUser
    {
        [Fact]
        public async void Returns_PatchedFileOut_when_ChangedParentDirectory()
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
                FileType = DAO.Models.Base.FileType.DIRECTORY,
                Name = "Directory",
                OwnerID = userId,
            };

            await databaseContext.Directories.AddAsync(directory);

            File fileToMove = new File
            {
                FileType = DAO.Models.Base.FileType.FILE,
                Name = "File.txt",
                OwnerID = userId
            };

            await databaseContext.Files.AddAsync(fileToMove);
            await databaseContext.SaveChangesAsync();

            JsonPatchDocument<FilePatch> jsonPatchDocument = new JsonPatchDocument<FilePatch>();
            jsonPatchDocument.Add(_ => _.ParentDirectoryID, directory.ID);

            FileOut result = await fileService.PatchByIdAndFilePatchAndUser(fileToMove.ID, jsonPatchDocument, "admin@admin.com");

            Assert.NotNull(result);
            Assert.Equal(directory.ID, result.ParentDirectoryID);
            Assert.Equal(fileToMove.Name, result.Name);
        }

        [Fact]
        public async void Returns_PatchedFileOut_when_ChangedName()
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
                FileType = DAO.Models.Base.FileType.FILE,
                Name = "File.txt",
                OwnerID = userId
            };

            await databaseContext.Files.AddAsync(fileToChange);
            await databaseContext.SaveChangesAsync();

            string newFileName = "newName.pdf";

            JsonPatchDocument<FilePatch> jsonPatchDocument = new JsonPatchDocument<FilePatch>();
            jsonPatchDocument.Add(_ => _.Name, newFileName);

            FileOut result = await fileService.PatchByIdAndFilePatchAndUser(fileToChange.ID, jsonPatchDocument, "admin@admin.com");

            Assert.NotNull(result);
            Assert.Equal(fileToChange.ParentDirectoryID, result.ParentDirectoryID);
            Assert.Equal(newFileName, result.Name);
        }

        [Fact]
        public async void Returns_PatchedFileOut_when_ChangedNameAndParentDirectory()
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
                FileType = DAO.Models.Base.FileType.DIRECTORY,
                Name = "Directory",
                OwnerID = userId,
            };

            await databaseContext.Directories.AddAsync(directory);

            File fileToChange = new File
            {
                FileType = DAO.Models.Base.FileType.FILE,
                Name = "File.txt",
                OwnerID = userId
            };

            await databaseContext.Files.AddAsync(fileToChange);
            await databaseContext.SaveChangesAsync();

            string newFileName = "newName.pdf";

            JsonPatchDocument<FilePatch> jsonPatchDocument = new JsonPatchDocument<FilePatch>();
            jsonPatchDocument.Add(_ => _.ParentDirectoryID, directory.ID);
            jsonPatchDocument.Add(_ => _.Name, newFileName);

            FileOut result = await fileService.PatchByIdAndFilePatchAndUser(fileToChange.ID, jsonPatchDocument, "admin@admin.com");

            Assert.NotNull(result);
            Assert.Equal(directory.ID, result.ParentDirectoryID);
            Assert.Equal(newFileName, result.Name);
        }

        [Fact]
        public async void Returns_Null_when_FileNotBelongsToUser()
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
                FileType = DAO.Models.Base.FileType.FILE,
                Name = "File.txt",
                OwnerID = userId
            };

            await databaseContext.Files.AddAsync(fileToChange);
            await databaseContext.SaveChangesAsync();

            JsonPatchDocument<FilePatch> jsonPatchDocument = new JsonPatchDocument<FilePatch>();
            jsonPatchDocument.Add(_ => _.Name, "newName");

            FileOut result = await fileService.PatchByIdAndFilePatchAndUser(fileToChange.ID, jsonPatchDocument, "user@user.com");

            Assert.Null(result);
        }

        [Fact]
        public async void Returns_Null_when_FileNotExist()
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

            FileOut result = await fileService.PatchByIdAndFilePatchAndUser(Guid.NewGuid(), jsonPatchDocument, "admin@admin.com");

            Assert.Null(result);
        }
    }

    public class FileServiceTest_PostByUser
    {
        [Fact]
        public async void Returns_ListOfFileUploadResult_when_UploadedMultipleFile()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration configuration = new MapperConfiguration(conf =>
            {

            });
            IMapper mapper = configuration.CreateMapper();

            IFileService fileService = new FileService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            Directory directory = new Directory
            {
                FileType = FileType.DIRECTORY,
                OwnerID = userId,
                Name = "Directory"
            };

            await databaseContext.Directories.AddAsync(directory);

            FilePost filePost = new FilePost
            {
                ParentDirectoryID = directory.ID,
                Files = new List<IFormFile>
                {
                    new FormFile(
                        new System.IO.MemoryStream(Encoding.UTF8.GetBytes("text")),
                        0, 4, "Data", "file1.txt"),
                    new FormFile(
                        new System.IO.MemoryStream(Encoding.UTF8.GetBytes("Loooong text")),
                        0, 12, "Data", "file2.txt")
                }
            };

            List<FileUploadResult> result = await fileService.PostByUser(filePost, "admin@admin.com");

            Assert.NotNull(result);
            Assert.Equal(filePost.Files.Count(), result.Count());

            Assert.True(databaseContext.Files
                .Any(_ => _.ParentDirectoryID == directory.ID && _.Name == "file1.txt"));
            Assert.True(databaseContext.Files
                .Any(_ => _.ParentDirectoryID == directory.ID && _.Name == "file2.txt"));
        }

        [Fact]
        public async void Returns_ListOfFileUploadResult_when_UploadedSingleFile()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration configuration = new MapperConfiguration(conf =>
            {

            });
            IMapper mapper = configuration.CreateMapper();

            IFileService fileService = new FileService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            Directory directory = new Directory
            {
                FileType = FileType.DIRECTORY,
                OwnerID = userId,
                Name = "Directory"
            };

            await databaseContext.Directories.AddAsync(directory);

            FilePost filePost = new FilePost
            {
                ParentDirectoryID = directory.ID,
                Files = new List<IFormFile>
                {
                    new FormFile(
                        new System.IO.MemoryStream(Encoding.UTF8.GetBytes("text")),
                        0, 4, "Data", "file1.txt")
                }
            };

            List<FileUploadResult> result = await fileService.PostByUser(filePost, "admin@admin.com");

            Assert.NotNull(result);
            Assert.Equal(filePost.Files.Count(), result.Count());

            Assert.True(databaseContext.Files
                .Any(_ => _.ParentDirectoryID == directory.ID && _.Name == "file1.txt"));
        }

        [Fact]
        public async void Returns_ListOfFileUploadResult_when_ParentDirectoryIsNull()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration configuration = new MapperConfiguration(conf =>
            {

            });
            IMapper mapper = configuration.CreateMapper();

            IFileService fileService = new FileService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            FilePost filePost = new FilePost
            {
                Files = new List<IFormFile>
                {
                    new FormFile(
                        new System.IO.MemoryStream(Encoding.UTF8.GetBytes("text")),
                        0, 4, "Data", "file1.txt")
                }
            };

            List<FileUploadResult> result = await fileService.PostByUser(filePost, "admin@admin.com");

            Assert.NotNull(result);
            Assert.Equal(filePost.Files.Count(), result.Count());

            Assert.True(databaseContext.Files
                .Any(_ => _.ParentDirectoryID == null && _.Name == "file1.txt"));
        }

        [Fact]
        public async void Returns_Null_when_ParentDirectoryNotExist()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration configuration = new MapperConfiguration(conf =>
            {

            });
            IMapper mapper = configuration.CreateMapper();

            IFileService fileService = new FileService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            FilePost filePost = new FilePost
            {
                ParentDirectoryID = Guid.NewGuid(),
                Files = new List<IFormFile>
                {
                    new FormFile(
                        new System.IO.MemoryStream(Encoding.UTF8.GetBytes("text")),
                        0, 4, "Data", "file1.txt")
                }
            };

            List<FileUploadResult> result = await fileService.PostByUser(filePost, "admin@admin.com");

            Assert.Null(result);
        }

        [Fact]
        public async void Returns_Null_when_ParentDirectoryNotBelongsToUser()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration configuration = new MapperConfiguration(conf =>
            {

            });
            IMapper mapper = configuration.CreateMapper();

            IFileService fileService = new FileService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == "admin@admin.com")).Id;

            Directory directory = new Directory
            {
                FileType = FileType.DIRECTORY,
                OwnerID = userId,
                Name = "Directory"
            };

            await databaseContext.Directories.AddAsync(directory);

            FilePost filePost = new FilePost
            {
                ParentDirectoryID = directory.ID,
                Files = new List<IFormFile>
                {
                    new FormFile(
                        new System.IO.MemoryStream(Encoding.UTF8.GetBytes("text")),
                        0, 4, "Data", "file1.txt")
                }
            };

            List<FileUploadResult> result = await fileService.PostByUser(filePost, "user@user.com");

            Assert.Null(result);
        }
    }
}
