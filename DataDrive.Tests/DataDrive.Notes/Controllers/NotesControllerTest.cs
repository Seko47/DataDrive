using DataDrive.DAO.Helpers.Communication;
using DataDrive.Notes.Controllers;
using DataDrive.Notes.Models.Out;
using DataDrive.Notes.Services;
using DataDrive.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DataDrive.Tests.DataDrive.Notes.Controllers
{
    public class NotesControllerTest_GetById
    {
        private readonly static string ADMIN_USERNAME = "admin@admin.com";

        [Fact]
        public async void Returns_OkObjectResult200_when_NoteExistAndBelongsToUser()
        {
            StatusCode<NoteOut> status = new StatusCode<NoteOut>(StatusCodes.Status200OK);
            Mock<INotesService> notesServiceMock = new Mock<INotesService>();
            notesServiceMock.Setup(_ => _.GetByIdAndUser(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(status));

            NotesController notesController = new NotesController(notesServiceMock.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));
            notesController.Authenticate(ADMIN_USERNAME);

            IActionResult result = await notesController.Get(Guid.NewGuid());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Returns_NoteOut_when_NoteExistAndBelongsToUser()
        {
            NoteOut note = new NoteOut
            {
                ID = Guid.NewGuid(),
                Title = "Note Title",
                Content = "Note Content",
                FileType = DAO.Models.Base.FileType.NOTE
            };

            Mock<INotesService> notesServiceMock = new Mock<INotesService>();
            notesServiceMock.Setup(_ => _.GetByIdAndUser(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new StatusCode<NoteOut>(StatusCodes.Status200OK, note)));

            NotesController notesController = new NotesController(notesServiceMock.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));
            notesController.Authenticate(ADMIN_USERNAME);

            IActionResult result = await notesController.Get(Guid.NewGuid());

            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);
            Assert.IsType<NoteOut>(okObjectResult.Value);
        }

        [Fact]
        public async void Returns_OkObjectResult200_when_NoteIsSharedForEveryone()
        {
            StatusCode<NoteOut> status = new StatusCode<NoteOut>(StatusCodes.Status200OK);

            Mock<INotesService> notesServiceMock = new Mock<INotesService>();
            notesServiceMock.Setup(_ => _.GetByIdAndUser(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(status));

            NotesController notesController = new NotesController(notesServiceMock.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));

            IActionResult result = await notesController.Get(Guid.NewGuid());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Returns_NoteOut_when_NoteIsSharedForEveryone()
        {
            NoteOut note = new NoteOut
            {
                ID = Guid.NewGuid(),
                Title = "Note Title",
                Content = "Note Content",
                IsShared = true,
                IsSharedForEveryone = true,
                FileType = DAO.Models.Base.FileType.NOTE
            };

            Mock<INotesService> notesServiceMock = new Mock<INotesService>();
            notesServiceMock.Setup(_ => _.GetByIdAndUser(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new StatusCode<NoteOut>(StatusCodes.Status200OK, note)));

            NotesController notesController = new NotesController(notesServiceMock.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));

            IActionResult result = await notesController.Get(Guid.NewGuid());

            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);
            Assert.IsType<NoteOut>(okObjectResult.Value);
        }

        [Fact]
        public async void Returns_OkObjectResult200_when_NoteIsSharedForLoggedUser()
        {
            StatusCode<NoteOut> status = new StatusCode<NoteOut>(StatusCodes.Status200OK);

            Mock<INotesService> notesServiceMock = new Mock<INotesService>();
            notesServiceMock.Setup(_ => _.GetByIdAndUser(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(status));

            NotesController notesController = new NotesController(notesServiceMock.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));
            notesController.Authenticate(ADMIN_USERNAME);

            IActionResult result = await notesController.Get(Guid.NewGuid());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Returns_NoteOut_when_NoteIsSharedForLoggedUser()
        {
            NoteOut note = new NoteOut
            {
                ID = Guid.NewGuid(),
                Title = "Note Title",
                Content = "Note Content",
                IsShared = true,
                IsSharedForUsers = true,
                FileType = DAO.Models.Base.FileType.NOTE
            };

            Mock<INotesService> notesServiceMock = new Mock<INotesService>();
            notesServiceMock.Setup(_ => _.GetByIdAndUser(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new StatusCode<NoteOut>(StatusCodes.Status200OK, note)));

            NotesController notesController = new NotesController(notesServiceMock.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));
            notesController.Authenticate(ADMIN_USERNAME);

            IActionResult result = await notesController.Get(Guid.NewGuid());

            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);
            Assert.IsType<NoteOut>(okObjectResult.Value);
        }

        [Fact]
        public async void Returns_NotFoundObjectResult404_when_NoteNotExist()
        {
            StatusCode<NoteOut> status = new StatusCode<NoteOut>(StatusCodes.Status404NotFound);
            Mock<INotesService> notesService = new Mock<INotesService>();
            notesService.Setup(_ => _.GetByIdAndUser(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(status));

            NotesController notesController = new NotesController(notesService.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));
            notesController.Authenticate(ADMIN_USERNAME);

            IActionResult result = await notesController.Get(Guid.NewGuid());

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }

    public class NotesControllerTest_Get
    {
        private readonly static string ADMIN_USERNAME = "admin@admin.com";

        [Fact]
        public async void Returns_OkObjectResult200_when_NotesExistAndBelongsToUser()
        {
            StatusCode<List<NoteOut>> status = new StatusCode<List<NoteOut>>(StatusCodes.Status200OK);
            Mock<INotesService> notesServiceMock = new Mock<INotesService>();
            notesServiceMock.Setup(_ => _.GetAllByUser(It.IsAny<string>()))
                .Returns(Task.FromResult(status));

            NotesController notesController = new NotesController(notesServiceMock.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));
            notesController.Authenticate(ADMIN_USERNAME);

            IActionResult result = await notesController.Get();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Returns_ListOfNoteOut_when_NotesExistAndBelongsToUser()
        {
            List<NoteOut> listOfNotes = new List<NoteOut>()
            {
                new NoteOut
                {
                    ID = Guid.NewGuid(),
                    Title = "Note1 Title",
                    Content = "Note1 Content",
                    FileType = DAO.Models.Base.FileType.NOTE
                },
                new NoteOut
                {
                    ID = Guid.NewGuid(),
                    Title = "Note2 Title",
                    Content = "Note2 Content"
                }
            };

            Mock<INotesService> notesServiceMock = new Mock<INotesService>();
            notesServiceMock.Setup(_ => _.GetAllByUser(It.IsAny<string>()))
                .Returns(Task.FromResult(new StatusCode<List<NoteOut>>(StatusCodes.Status200OK, listOfNotes)));

            NotesController notesController = new NotesController(notesServiceMock.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));
            notesController.Authenticate(ADMIN_USERNAME);

            IActionResult result = await notesController.Get();

            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);
            Assert.IsType<List<NoteOut>>(okObjectResult.Value);
        }

        [Fact]
        public async void Returns_NotFoundObjectResult404_when_NoteNotExist()
        {
            StatusCode<List<NoteOut>> status = new StatusCode<List<NoteOut>>(StatusCodes.Status404NotFound);
            Mock<INotesService> notesService = new Mock<INotesService>();
            notesService.Setup(_ => _.GetAllByUser(It.IsAny<string>()))
                .Returns(Task.FromResult(status));

            NotesController notesController = new NotesController(notesService.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));
            notesController.Authenticate(ADMIN_USERNAME);

            IActionResult result = await notesController.Get();

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
