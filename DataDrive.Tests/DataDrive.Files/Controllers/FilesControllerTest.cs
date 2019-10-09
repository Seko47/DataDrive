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
                .Returns(files.Object);

            FilesController filesController = new FilesController(fileService.Object);
            filesController.Authenticate("admin");

            IActionResult result = await filesController.Get();

            Assert.IsType<OkObjectResult>(result);
        }
    }
}
