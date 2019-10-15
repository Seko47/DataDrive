﻿using DataDrive.Files.Controllers;
using DataDrive.Files.Models.In;
using DataDrive.Files.Models.Out;
using DataDrive.Files.Services;
using DataDrive.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataDrive.Tests.DataDrive.Files.Controllers
{
    public class FilesControllerTest_GetById
    {
        [Fact]
        public async void Returns_OkObjectResult200_when_FileExistAndBelongsToUser()
        {
            Mock<FileOut> file = new Mock<FileOut>();
            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(_ => _.GetByIdAndUser(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(file.Object));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin@admin.com");

            IActionResult result = await filesController.Get(Guid.NewGuid());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Returns_FileOut_when_FileExistAndBelongsToUser()
        {
            FileOut file = new FileOut
            {
                ID = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString()
            };

            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(_ => _.GetByIdAndUser(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(file));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin@admin.com");

            IActionResult result = await filesController.Get(Guid.NewGuid());
            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);
            Assert.IsType<FileOut>(okObjectResult.Value);
        }

        [Fact]
        public async void Returns_NotFoundObjectResult404_when_FileNotExistOrNotBelongsToUser()
        {
            FileOut file = null;
            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(_ => _.GetByIdAndUser(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(file));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin@admin.com");

            IActionResult result = await filesController.Get(Guid.NewGuid());

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }

    public class FilesControllerTest_GetFromDirectoryById
    {
        [Fact]
        public async void Returns_OkObjectResult200_when_DirectoryExistAndBelongsToUser()
        {
            Mock<DirectoryOut> directory = new Mock<DirectoryOut>();
            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(_ => _.GetDirectoryByIdAndUser(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(directory.Object));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin@admin.com");

            IActionResult result = await filesController.GetFromDirectory(Guid.NewGuid());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Returns_DirectoryOut_when_DirectoryExistAndBelongsToUser()
        {
            DirectoryOut directory = new DirectoryOut
            {
                ID = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Files = new List<FileOut>()
                {
                    new FileOut()
                    {
                        ID = Guid.NewGuid(),
                        Name = Guid.NewGuid().ToString()
                    }
                }
            };

            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(_ => _.GetDirectoryByIdAndUser(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(directory));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin@admin.com");

            IActionResult result = await filesController.GetFromDirectory(Guid.NewGuid());
            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);
            Assert.IsType<DirectoryOut>(okObjectResult.Value);
        }

        [Fact]
        public async void Returns_NotFoundObjectResult404_when_DirectoryNotExistOrNotBelongsToUser()
        {
            DirectoryOut directory = null;
            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(_ => _.GetDirectoryByIdAndUser(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(directory));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin@admin.com");

            IActionResult result = await filesController.GetFromDirectory(Guid.NewGuid());

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }

    public class FilesControllerTest_DownloadById
    {
        [Fact]
        public async void Returns_OkObjectResult200_when_FileExistAndBelongsToUser()
        {
            string contentType = "application/octet-stream";
            byte[] content = Encoding.UTF8.GetBytes("File content ...");
            string fileName = "file.txt";

            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(_ => _.DownloadByIdAndUser(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new Tuple<string, byte[], string>(fileName, content, contentType)));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin@admin.com");

            IActionResult result = await filesController.Download(Guid.NewGuid());

            Assert.IsType<FileContentResult>(result);
        }

        [Fact]
        public async void Returns_DownloadingFile_when_FileExistAndBelongsToUser()
        {
            string contentType = "application/octet-stream";
            byte[] content = Encoding.UTF8.GetBytes("File content ...");
            string fileName = "file.txt";

            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(_ => _.DownloadByIdAndUser(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new Tuple<string, byte[], string>(fileName, content, contentType)));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin@admin.com");

            IActionResult result = await filesController.Download(Guid.NewGuid());

            FileContentResult fileContentResult = result as FileContentResult;
            Assert.NotNull(fileContentResult);

            Assert.NotNull(fileContentResult.FileDownloadName);
            Assert.NotNull(fileContentResult.FileContents);
            Assert.NotNull(fileContentResult.ContentType);
        }

        [Fact]
        public async void Returns_NotFoundObjectResult404_when_FileNotExistOrNotBelongsToUSer()
        {
            Tuple<string, byte[], string> tuple = null;

            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(_ => _.DownloadByIdAndUser(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(tuple));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin@admin.com");

            IActionResult result = await filesController.Download(Guid.NewGuid());
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }

    public class FilesControllerTest_DeleteById
    {
        [Fact]
        public async void Returns_OkObjectResult200_when_FileExistAndBelongsToUser()
        {
            Mock<DirectoryOut> parentDirectoryMock = new Mock<DirectoryOut>();
            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(_ => _.DeleteByIdAndUser(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(parentDirectoryMock.Object));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin@admin.com");

            IActionResult result = await filesController.Delete(Guid.NewGuid());
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Returns_ParentDirectoryOfDeletingFile_when_FileExistAndBelongsToUser()
        {
            Guid parentId = Guid.NewGuid();
            DirectoryOut parentDirectoryMock = new DirectoryOut
            {
                ID = parentId
            };

            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(_ => _.DeleteByIdAndUser(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(parentDirectoryMock));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin@admin.com");

            IActionResult result = await filesController.Delete(Guid.NewGuid());
            OkObjectResult okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            DirectoryOut parentDirectory = okObjectResult.Value as DirectoryOut;
            Assert.NotNull(parentDirectory);
            Assert.Equal(parentId, parentDirectory.ID);
        }

        [Fact]
        public async void Returns_NotFoundObjectResult404_when_FileNotExistOrNotBelongsToUser()
        {
            DirectoryOut parentDirectory = null;
            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(_ => _.DeleteByIdAndUser(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(parentDirectory));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin@admin.com");

            IActionResult result = await filesController.Delete(Guid.NewGuid());
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }

    public class FilesControllerTest_PatchById
    {
        [Fact]
        public async void Returns_OkObjectResult200()
        {
            Mock<FileOut> file = new Mock<FileOut>();
            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(_ => _.PatchByIdAndFilePatchAndUser(It.IsAny<Guid>(), It.IsAny<JsonPatchDocument<FilePatch>>(), It.IsAny<string>()))
                .Returns(Task.FromResult(file.Object));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin@admin.com");

            IActionResult result = await filesController.Patch(Guid.NewGuid(), new JsonPatchDocument<FilePatch>());
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Returns_PatchedFileOut()
        {
            FileOut file = new FileOut
            {
                ID = Guid.NewGuid()
            };
            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(_ => _.PatchByIdAndFilePatchAndUser(It.IsAny<Guid>(), It.IsAny<JsonPatchDocument<FilePatch>>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(file));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin@admin.com");

            IActionResult result = await filesController.Patch(Guid.NewGuid(), new JsonPatchDocument<FilePatch>());
            OkObjectResult okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.IsType<FileOut>(okObjectResult.Value);
        }

        [Fact]
        public async void Returns_NotFoundObjectResult404_when_FileNotExistOrNotBelongsToUser()
        {
            FileOut file = null;
            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(_ => _.PatchByIdAndFilePatchAndUser(It.IsAny<Guid>(), It.IsAny<JsonPatchDocument<FilePatch>>(), It.IsAny<string>()))
                .Returns(Task.FromResult(file));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin@admin.com");

            IActionResult result = await filesController.Patch(Guid.NewGuid(), new JsonPatchDocument<FilePatch>());
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }

    public class FilesControllerTest_Post
    {
        [Fact]
        public async void Returns_OkObjectResult200()
        {
            Mock<List<FileUploadResult>> fileUploadResults = new Mock<List<FileUploadResult>>();
            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(_ => _.PostByUser(It.IsAny<FilePost>(), It.IsAny<string>()))
                .Returns(Task.FromResult(fileUploadResults.Object));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin@admin.com");

            FilePost filePost = new FilePost
            {
                ParentDirectoryID = null,
                Files = new List<IFormFile>
                {
                    new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Some content")), 0,12, "file","file.txt")
                }
            };

            IActionResult result = await filesController.Post(filePost);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Returns_ListFileUploadResult()
        {
            List<FileUploadResult> fileUploadResults = new List<FileUploadResult>();
            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(_ => _.PostByUser(It.IsAny<FilePost>(), It.IsAny<string>()))
                .Returns(Task.FromResult(fileUploadResults));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin@admin.com");

            FilePost filePost = new FilePost
            {
                ParentDirectoryID = null,
                Files = new List<IFormFile>
                {
                    new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Some content")), 0,12, "file","file.txt")
                }
            };

            IActionResult result = await filesController.Post(filePost);
            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);

            Assert.IsType<List<FileUploadResult>>(okObjectResult.Value);
        }

        [Fact]
        public async void Returns_BadRequestObjectResult_when_FilePostFilesEqualNull()
        {
            Mock<List<FileUploadResult>> fileUploadResults = new Mock<List<FileUploadResult>>();
            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(_ => _.PostByUser(It.IsAny<FilePost>(), It.IsAny<string>()))
                .Returns(Task.FromResult(fileUploadResults.Object));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin@admin.com");

            FilePost filePost = new FilePost
            {
                ParentDirectoryID = Guid.NewGuid(),
                Files = null
            };
            IActionResult result = await filesController.Post(filePost);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void Returns_BadRequestObjectResult_when_FilePostFilesEmpty()
        {
            Mock<List<FileUploadResult>> fileUploadResults = new Mock<List<FileUploadResult>>();
            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(_ => _.PostByUser(It.IsAny<FilePost>(), It.IsAny<string>()))
                .Returns(Task.FromResult(fileUploadResults.Object));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin@admin.com");

            FilePost filePost = new FilePost
            {
                ParentDirectoryID = Guid.NewGuid(),
                Files = new List<IFormFile>()
            };
            IActionResult result = await filesController.Post(filePost);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void Returns_NotFoundObjectResult_when_ParentDirectoryNotExistsOrNotBelongsToUser()
        {
            List<FileUploadResult> fileUploadResults = null;
            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(_ => _.PostByUser(It.IsAny<FilePost>(), It.IsAny<string>()))
                .Returns(Task.FromResult(fileUploadResults));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin@admin.com");

            FilePost filePost = new FilePost
            {
                ParentDirectoryID = Guid.NewGuid(),
                Files = new List<IFormFile>()
                {
                    new FormFile(null, 0, 0, "", "")
                }
            };
            IActionResult result = await filesController.Post(filePost);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }

    public class FilesControllerTest_CreateDirectory
    {
        [Fact]
        public async void Returns_CreatedAtActionResult201()
        {
            Mock<DirectoryOut> directoryOut = new Mock<DirectoryOut>();
            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(_ => _.CreateDirectoryByUser(It.IsAny<DirectoryPost>(), It.IsAny<string>()))
                .Returns(Task.FromResult(directoryOut.Object));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin@admin.com");

            IActionResult result = await filesController.CreateDirectory(new DirectoryPost());

            Assert.IsType<CreatedAtActionResult>(result);
        }
        [Fact]
        public async void Returns_NotFoundObjectResult_when_ParentDirectoryNotExistsOrNotBelongsToUser()
        {
            DirectoryOut directoryOut = null;
            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(_ => _.CreateDirectoryByUser(It.IsAny<DirectoryPost>(), It.IsAny<string>()))
                .Returns(Task.FromResult(directoryOut));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin@admin.com");

            IActionResult result = await filesController.CreateDirectory(new DirectoryPost());

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}