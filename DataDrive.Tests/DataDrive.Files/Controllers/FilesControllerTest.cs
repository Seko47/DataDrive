using DataDrive.Files.Controllers;
using DataDrive.Files.Models.Out;
using DataDrive.Files.Services;
using DataDrive.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataDrive.Tests.DataDrive.Files.Controllers
{
    public class FilesControllerTest_Get
    {
        [Fact]
        public async void Returns_OkObjectResult200()
        {
            Mock<List<FileOut>> files = new Mock<List<FileOut>>();
            Mock<IFileService> fileService = new Mock<IFileService>();

            fileService.Setup(_ => _.GetAllFromRootByUser(It.IsAny<string>()))
                .Returns(Task.FromResult(files.Object));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin");

            IActionResult result = await filesController.Get();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Returns_FileOutList()
        {
            List<FileOut> files = new List<FileOut>()
            {
                new FileOut
                {
                    ID = Guid.NewGuid()
                },
                new FileOut
                {
                    ID = Guid.NewGuid()
                }
            };

            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(_ => _.GetAllFromRootByUser(It.IsAny<string>()))
                .Returns(Task.FromResult(files));

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin");

            IActionResult result = await filesController.Get();
            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);

            List<FileOut> value = okObjectResult.Value as List<FileOut>;
            Assert.IsType<List<FileOut>>(value);
            Assert.True(value.Count == 2);
        }
    }

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
            filesController.Authenticate("admin");

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
            filesController.Authenticate("admin");

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
            filesController.Authenticate("admin");

            IActionResult result = await filesController.Get(Guid.NewGuid());

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
