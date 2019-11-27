using DataDrive.DAO.Helpers.Communication;
using DataDrive.Notes.Controllers;
using DataDrive.Notes.Models.In;
using DataDrive.Notes.Models.Out;
using DataDrive.Notes.Services;
using DataDrive.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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

    public class NotesControllerTest_DeleteById
    {
        private readonly static string ADMIN_USERNAME = "admin@admin.com";

        [Fact]
        public async void Returns_OkObjectResult200_when_NoteDeleted()
        {
            StatusCode<Guid> status = new StatusCode<Guid>(StatusCodes.Status200OK);

            Mock<INotesService> notesServiceMock = new Mock<INotesService>();
            notesServiceMock.Setup(_ => _.DeleteByIdAndUser(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(status));

            NotesController notesController = new NotesController(notesServiceMock.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));
            notesController.Authenticate(ADMIN_USERNAME);

            IActionResult result = await notesController.Delete(Guid.NewGuid());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Returns_IdOfDeletedNote_when_NoteDeleted()
        {
            Guid noteId = Guid.NewGuid();

            StatusCode<Guid> status = new StatusCode<Guid>(StatusCodes.Status200OK, noteId);

            Mock<INotesService> notesServiceMock = new Mock<INotesService>();
            notesServiceMock.Setup(_ => _.DeleteByIdAndUser(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(status));

            NotesController notesController = new NotesController(notesServiceMock.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));
            notesController.Authenticate(ADMIN_USERNAME);

            IActionResult result = await notesController.Delete(noteId);

            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);
            Assert.Equal(noteId, okObjectResult.Value);
        }
    }

    public class NotesControllerTest_PatchById
    {
        private readonly static string ADMIN_USERNAME = "admin@admin.com";

        [Fact]
        public async void Returns_OkObjectResult200_when_Success()
        {
            StatusCode<NoteOut> status = new StatusCode<NoteOut>(StatusCodes.Status200OK);

            Mock<INotesService> notesServiceMock = new Mock<INotesService>();
            notesServiceMock.Setup(_ => _.PatchByIdAndFilePatchAndUser(It.IsAny<Guid>(), It.IsAny<JsonPatchDocument<NotePatch>>(), It.IsAny<string>()))
                .Returns(Task.FromResult(status));

            NotesController notesController = new NotesController(notesServiceMock.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));
            notesController.Authenticate(ADMIN_USERNAME);

            IActionResult result = await notesController.Patch(Guid.NewGuid(), new JsonPatchDocument<NotePatch>());
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Returns_PatchedNoteOut_when_Success()
        {
            NoteOut note = new NoteOut
            {
                ID = Guid.NewGuid()
            };

            StatusCode<NoteOut> status = new StatusCode<NoteOut>(StatusCodes.Status200OK, note);

            Mock<INotesService> notesServiceMock = new Mock<INotesService>();
            notesServiceMock.Setup(_ => _.PatchByIdAndFilePatchAndUser(It.IsAny<Guid>(), It.IsAny<JsonPatchDocument<NotePatch>>(), It.IsAny<string>()))
                .Returns(Task.FromResult(status));

            NotesController notesController = new NotesController(notesServiceMock.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));
            notesController.Authenticate(ADMIN_USERNAME);

            IActionResult result = await notesController.Patch(Guid.NewGuid(), new JsonPatchDocument<NotePatch>());
            OkObjectResult okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.IsType<NoteOut>(okObjectResult.Value);
        }

        [Fact]
        public async void Returns_NotFoundObjectResult404_when_NoteNotExist()
        {
            StatusCode<NoteOut> status = new StatusCode<NoteOut>(StatusCodes.Status404NotFound);

            Mock<INotesService> notesServiceMock = new Mock<INotesService>();
            notesServiceMock.Setup(_ => _.PatchByIdAndFilePatchAndUser(It.IsAny<Guid>(), It.IsAny<JsonPatchDocument<NotePatch>>(), It.IsAny<string>()))
                .Returns(Task.FromResult(status));

            NotesController notesController = new NotesController(notesServiceMock.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));
            notesController.Authenticate(ADMIN_USERNAME);

            IActionResult result = await notesController.Patch(Guid.NewGuid(), new JsonPatchDocument<NotePatch>());
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }

    public class NotesControllerTest_Post
    {
        private readonly static string ADMIN_USERNAME = "admin@admin.com";

        [Fact]
        public async void Returns_OkObjectResult200_when_Success()
        {
            StatusCode<NoteOut> status = new StatusCode<NoteOut>(StatusCodes.Status200OK);
            Mock<INotesService> notesServiceMock = new Mock<INotesService>();
            notesServiceMock.Setup(_ => _.PostNoteByUser(It.IsAny<NotePost>(), It.IsAny<string>()))
                .Returns(Task.FromResult(status));

            NotesController notesController = new NotesController(notesServiceMock.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));
            notesController.Authenticate(ADMIN_USERNAME);

            NotePost notePost = new NotePost
            {
                Title = "New note's title",
                Content = "New note's content"
            };

            IActionResult result = await notesController.Post(notePost);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Returns_NoteOut_when_Success()
        {
            NotePost notePost = new NotePost
            {
                Title = "New note's title",
                Content = "New note's content"
            };

            NoteOut noteOut = new NoteOut
            {
                ID = Guid.NewGuid(),
                Title = "New note's title",
                Content = "New note's content"
            };

            StatusCode<NoteOut> status = new StatusCode<NoteOut>(StatusCodes.Status200OK, noteOut);
            Mock<INotesService> notesServiceMock = new Mock<INotesService>();
            notesServiceMock.Setup(_ => _.PostNoteByUser(It.IsAny<NotePost>(), It.IsAny<string>()))
                .Returns(Task.FromResult(status));

            NotesController notesController = new NotesController(notesServiceMock.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));
            notesController.Authenticate(ADMIN_USERNAME);

            IActionResult result = await notesController.Post(notePost);
            OkObjectResult okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.IsType<NoteOut>(okObjectResult.Value);
        }
    }
}
