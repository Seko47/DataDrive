using AutoMapper;
using DataDrive.DAO.Context;
using DataDrive.DAO.Helpers.Communication;
using DataDrive.DAO.Models;
using DataDrive.Notes.Models.In;
using DataDrive.Notes.Models.Out;
using DataDrive.Notes.Services;
using DataDrive.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DataDrive.Tests.DataDrive.Notes.Services
{
    public class NotesServiceTest_DeleteByIdAndUser
    {
        private readonly static string ADMIN_USERNAME = "admin@admin.com";

        [Fact]
        public async void Returns_OkObjectResult200AndIdOfDeletedNote_when_NoteDeleted()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Note_to_NoteOut());
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            INotesService notesService = new NotesService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ADMIN_USERNAME)).Id;

            Note noteToDelete = new Note
            {
                FileType = DAO.Models.Base.FileType.NOTE,
                Title = "Note's title",
                Content = "Note's content",
                OwnerID = userId,
                CreatedDateTime = DateTime.Now
            };

            await databaseContext.Notes.AddAsync(noteToDelete);
            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Notes.Any(_ => _.ID == noteToDelete.ID));

            StatusCode<Guid> status = await notesService.DeleteByIdAndUser(noteToDelete.ID, ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status200OK, status.Code);
            Assert.Equal(noteToDelete.ID, status.Body);
            Assert.False(databaseContext.Notes.Any(_ => _.ID == noteToDelete.ID));
        }

        [Fact]
        public async void Returns_NotFoundObjectResult404_when_NoteNotBelongsToLoggedUser()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Note_to_NoteOut());
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            INotesService notesService = new NotesService(databaseContext, mapper);

            Note noteToDelete = new Note
            {
                FileType = DAO.Models.Base.FileType.NOTE,
                Title = "Note's title",
                Content = "Note's content",
                OwnerID = Guid.NewGuid().ToString(),
                CreatedDateTime = DateTime.Now
            };

            await databaseContext.Notes.AddAsync(noteToDelete);
            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Notes.Any(_ => _.ID == noteToDelete.ID));

            StatusCode<Guid> status = await notesService.DeleteByIdAndUser(noteToDelete.ID, ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status404NotFound, status.Code);
            Assert.True(databaseContext.Notes.Any(_ => _.ID == noteToDelete.ID));
        }

        [Fact]
        public async void Returns_NotFoundObjectResult404_when_NoteNotExists()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Note_to_NoteOut());
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            INotesService notesService = new NotesService(databaseContext, mapper);

            StatusCode<Guid> status = await notesService.DeleteByIdAndUser(Guid.NewGuid(), ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status404NotFound, status.Code);
        }
    }

    public class NotesServiceTest_GetAllByUser
    {
        private readonly static string ADMIN_USERNAME = "admin@admin.com";

        [Fact]
        public async void Returns_OkObjectResult200AndListOfNoteOut_when_LoggedUserHasNotes()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Note_to_NoteOut());
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            INotesService notesService = new NotesService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ADMIN_USERNAME)).Id;

            Note note1 = new Note
            {
                FileType = DAO.Models.Base.FileType.NOTE,
                Title = "Note1's title",
                Content = "Note1's content",
                OwnerID = userId,
                CreatedDateTime = DateTime.Now
            };

            Note note2 = new Note
            {
                FileType = DAO.Models.Base.FileType.NOTE,
                Title = "Note1's title",
                Content = "Note1's content",
                OwnerID = userId,
                CreatedDateTime = DateTime.Now
            };

            await databaseContext.Notes.AddRangeAsync(note1, note2);
            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Notes.Any(_ => _.ID == note1.ID));
            Assert.True(databaseContext.Notes.Any(_ => _.ID == note2.ID));

            StatusCode<List<NoteOut>> status = await notesService.GetAllByUser(ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status200OK, status.Code);
            Assert.NotNull(status.Body);
            Assert.IsType<List<NoteOut>>(status.Body);
            Assert.NotEmpty(status.Body);
        }

        [Fact]
        public async void Returns_NotFoundObjectResult404_when_LoggedUserHasNotNotes()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Note_to_NoteOut());
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            INotesService notesService = new NotesService(databaseContext, mapper);

            Note note1 = new Note
            {
                FileType = DAO.Models.Base.FileType.NOTE,
                Title = "Note1's title",
                Content = "Note1's content",
                OwnerID = Guid.NewGuid().ToString(),
                CreatedDateTime = DateTime.Now
            };

            Note note2 = new Note
            {
                FileType = DAO.Models.Base.FileType.NOTE,
                Title = "Note1's title",
                Content = "Note1's content",
                OwnerID = Guid.NewGuid().ToString(),
                CreatedDateTime = DateTime.Now
            };

            await databaseContext.Notes.AddRangeAsync(note1, note2);
            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Notes.Any(_ => _.ID == note1.ID));
            Assert.True(databaseContext.Notes.Any(_ => _.ID == note2.ID));

            StatusCode<List<NoteOut>> status = await notesService.GetAllByUser(ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status404NotFound, status.Code);
        }
    }

    public class NotesServiceTest_GetByIdAndUser
    {
        private readonly static string ADMIN_USERNAME = "admin@admin.com";
        private readonly static string USER_USERNAME = "user@user.com";

        [Fact]
        public async void Returns_OkObjectResult200AndNoteOut_when_NoteExistsAndBelongsToUser()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Note_to_NoteOut());
            });
            IMapper mapper = mapperConfiguration.CreateMapper();

            INotesService notesService = new NotesService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ADMIN_USERNAME)).Id;

            Note note = new Note
            {
                FileType = DAO.Models.Base.FileType.NOTE,
                Title = "Note's title",
                Content = "Note's content",
                OwnerID = userId,
                CreatedDateTime = DateTime.Now
            };

            await databaseContext.Notes.AddAsync(note);
            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Notes.Any(_ => _.ID == note.ID));

            StatusCode<NoteOut> status = await notesService.GetByIdAndUser(note.ID, ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status200OK, status.Code);
            Assert.NotNull(status.Body);
            Assert.IsType<NoteOut>(status.Body);
        }

        [Fact]
        public async void Returns_OkObjectResult200AndNoteOut_when_NoteExistsAndIsSharedForEveryone()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Note_to_NoteOut());
            });
            IMapper mapper = mapperConfiguration.CreateMapper();

            INotesService notesService = new NotesService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ADMIN_USERNAME)).Id;

            Note note = new Note
            {
                FileType = DAO.Models.Base.FileType.NOTE,
                Title = "Note's title",
                Content = "Note's content",
                OwnerID = userId,
                CreatedDateTime = DateTime.Now,
                IsShared = true,
                IsSharedForEveryone = true
            };

            await databaseContext.Notes.AddAsync(note);

            ShareEveryone shareEveryone = new ShareEveryone
            {
                CreatedDateTime = DateTime.Now,
                FileID = note.ID,
                OwnerID = userId,
                Token = "XYZ"
            };

            await databaseContext.ShareEveryones.AddAsync(shareEveryone);

            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Notes
                .Any(_ => _.ID == note.ID));

            Assert.True(databaseContext.ShareEveryones
                .Any(_ => _.ID == shareEveryone.ID));

            StatusCode<NoteOut> status = await notesService.GetByIdAndUser(note.ID, USER_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status200OK, status.Code);
            Assert.NotNull(status.Body);
            Assert.IsType<NoteOut>(status.Body);
        }

        [Fact]
        public async void Returns_OkObjectResult200AndNoteOut_when_NoteExistsAndIsSharedForEveryoneAndDownloadLimitIsGreaterThan0()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Note_to_NoteOut());
            });
            IMapper mapper = mapperConfiguration.CreateMapper();

            INotesService notesService = new NotesService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ADMIN_USERNAME)).Id;

            Note note = new Note
            {
                FileType = DAO.Models.Base.FileType.NOTE,
                Title = "Note's title",
                Content = "Note's content",
                OwnerID = userId,
                CreatedDateTime = DateTime.Now,
                IsShared = true,
                IsSharedForEveryone = true
            };

            await databaseContext.Notes.AddAsync(note);

            int downloadLimit = 5;

            ShareEveryone shareEveryone = new ShareEveryone
            {
                CreatedDateTime = DateTime.Now,
                FileID = note.ID,
                OwnerID = userId,
                Token = "XYZ",
                DownloadLimit = downloadLimit
            };

            await databaseContext.ShareEveryones.AddAsync(shareEveryone);

            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Notes
                .Any(_ => _.ID == note.ID));

            Assert.True(databaseContext.ShareEveryones
                .Any(_ => _.ID == shareEveryone.ID));

            StatusCode<NoteOut> status = await notesService.GetByIdAndUser(note.ID, USER_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status200OK, status.Code);
            Assert.NotNull(status.Body);
            Assert.IsType<NoteOut>(status.Body);
            Assert.Equal(downloadLimit, shareEveryone.DownloadLimit);
        }

        [Fact]
        public async void Returns_OkObjectResult200AndNoteOut_when_NoteExistsAndIsSharedForEveryoneAndExpirationDateTimeNotEnds()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Note_to_NoteOut());
            });
            IMapper mapper = mapperConfiguration.CreateMapper();

            INotesService notesService = new NotesService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ADMIN_USERNAME)).Id;

            Note note = new Note
            {
                FileType = DAO.Models.Base.FileType.NOTE,
                Title = "Note's title",
                Content = "Note's content",
                OwnerID = userId,
                CreatedDateTime = DateTime.Now,
                IsShared = true,
                IsSharedForEveryone = true
            };

            await databaseContext.Notes.AddAsync(note);

            ShareEveryone shareEveryone = new ShareEveryone
            {
                CreatedDateTime = DateTime.Now,
                FileID = note.ID,
                OwnerID = userId,
                Token = "XYZ",
                ExpirationDateTime = DateTime.Now.AddDays(1)
            };

            await databaseContext.ShareEveryones.AddAsync(shareEveryone);

            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Notes
                .Any(_ => _.ID == note.ID));

            Assert.True(databaseContext.ShareEveryones
                .Any(_ => _.ID == shareEveryone.ID));

            StatusCode<NoteOut> status = await notesService.GetByIdAndUser(note.ID, USER_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status200OK, status.Code);
            Assert.NotNull(status.Body);
            Assert.IsType<NoteOut>(status.Body);
        }

        [Fact]
        public async void Returns_OkObjectResult200AndNoteOut_when_NoteExistsAndIsSharedForLoggedUserAndExpirationDateTimeIsNotSet()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Note_to_NoteOut());
            });
            IMapper mapper = mapperConfiguration.CreateMapper();

            INotesService notesService = new NotesService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ADMIN_USERNAME)).Id;

            string ownerId = Guid.NewGuid().ToString();

            Note note = new Note
            {
                FileType = DAO.Models.Base.FileType.NOTE,
                Title = "Note's title",
                Content = "Note's content",
                OwnerID = ownerId,
                CreatedDateTime = DateTime.Now,
                IsShared = true,
                IsSharedForUsers = true
            };

            await databaseContext.Notes.AddAsync(note);

            ShareForUser shareForUser = new ShareForUser
            {
                CreatedDateTime = DateTime.Now,
                FileID = note.ID,
                OwnerID = ownerId,
                SharedForUserID = userId
            };

            await databaseContext.ShareForUsers.AddAsync(shareForUser);

            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Notes
                .Any(_ => _.ID == note.ID));

            Assert.True(databaseContext.ShareForUsers
                .Any(_ => _.ID == shareForUser.ID));

            StatusCode<NoteOut> status = await notesService.GetByIdAndUser(note.ID, ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status200OK, status.Code);
            Assert.NotNull(status.Body);
            Assert.IsType<NoteOut>(status.Body);
        }

        [Fact]
        public async void Returns_OkObjectResult200AndNoteOut_when_NoteExistsAndIsSharedForLoggedUserAndExpirationDateTimeNotEnds()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Note_to_NoteOut());
            });
            IMapper mapper = mapperConfiguration.CreateMapper();

            INotesService notesService = new NotesService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ADMIN_USERNAME)).Id;

            string ownerId = Guid.NewGuid().ToString();

            Note note = new Note
            {
                FileType = DAO.Models.Base.FileType.NOTE,
                Title = "Note's title",
                Content = "Note's content",
                OwnerID = ownerId,
                CreatedDateTime = DateTime.Now,
                IsShared = true,
                IsSharedForUsers = true
            };

            await databaseContext.Notes.AddAsync(note);

            ShareForUser shareForUser = new ShareForUser
            {
                CreatedDateTime = DateTime.Now,
                FileID = note.ID,
                OwnerID = ownerId,
                SharedForUserID = userId,
                ExpirationDateTime = DateTime.Now.AddDays(1)
            };

            await databaseContext.ShareForUsers.AddAsync(shareForUser);

            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Notes
                .Any(_ => _.ID == note.ID));

            Assert.True(databaseContext.ShareForUsers
                .Any(_ => _.ID == shareForUser.ID));

            StatusCode<NoteOut> status = await notesService.GetByIdAndUser(note.ID, ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status200OK, status.Code);
            Assert.NotNull(status.Body);
            Assert.IsType<NoteOut>(status.Body);
        }

        [Fact]
        public async void Returns_NotFoundObjectResult404_when_NoteExistsAndIsSharedForEveryoneAndExpirationDateTimeEnds()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Note_to_NoteOut());
            });
            IMapper mapper = mapperConfiguration.CreateMapper();

            INotesService notesService = new NotesService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ADMIN_USERNAME)).Id;

            Note note = new Note
            {
                FileType = DAO.Models.Base.FileType.NOTE,
                Title = "Note's title",
                Content = "Note's content",
                OwnerID = userId,
                CreatedDateTime = DateTime.Now,
                IsShared = true,
                IsSharedForEveryone = true
            };

            await databaseContext.Notes.AddAsync(note);

            ShareEveryone shareEveryone = new ShareEveryone
            {
                CreatedDateTime = DateTime.Now,
                FileID = note.ID,
                OwnerID = userId,
                Token = "XYZ",
                ExpirationDateTime = DateTime.Now.AddDays(-1)
            };

            await databaseContext.ShareEveryones.AddAsync(shareEveryone);

            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Notes
                .Any(_ => _.ID == note.ID));

            Assert.True(databaseContext.ShareEveryones
                .Any(_ => _.ID == shareEveryone.ID));

            StatusCode<NoteOut> status = await notesService.GetByIdAndUser(note.ID, USER_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status404NotFound, status.Code);
        }

        [Fact]
        public async void Returns_NotFoundObjectResult404_when_NoteExistsAndIsSharedForEveryoneAndDownloadLimitIsEqual0()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Note_to_NoteOut());
            });
            IMapper mapper = mapperConfiguration.CreateMapper();

            INotesService notesService = new NotesService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ADMIN_USERNAME)).Id;

            Note note = new Note
            {
                FileType = DAO.Models.Base.FileType.NOTE,
                Title = "Note's title",
                Content = "Note's content",
                OwnerID = userId,
                CreatedDateTime = DateTime.Now,
                IsShared = true,
                IsSharedForEveryone = true
            };

            await databaseContext.Notes.AddAsync(note);

            ShareEveryone shareEveryone = new ShareEveryone
            {
                CreatedDateTime = DateTime.Now,
                FileID = note.ID,
                OwnerID = userId,
                Token = "XYZ",
                DownloadLimit = 0
            };

            await databaseContext.ShareEveryones.AddAsync(shareEveryone);

            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Notes
                .Any(_ => _.ID == note.ID));

            Assert.True(databaseContext.ShareEveryones
                .Any(_ => _.ID == shareEveryone.ID));

            StatusCode<NoteOut> status = await notesService.GetByIdAndUser(note.ID, USER_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status404NotFound, status.Code);
        }

        [Fact]
        public async void Returns_NotFoundObjectResult404_when_NoteExistsAndIsSharedForLoggedUserAndExpirationDateTimeEnds()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Note_to_NoteOut());
            });
            IMapper mapper = mapperConfiguration.CreateMapper();

            INotesService notesService = new NotesService(databaseContext, mapper);

            string userId = (await databaseContext.Users.FirstOrDefaultAsync(_ => _.UserName == ADMIN_USERNAME)).Id;

            string ownerId = Guid.NewGuid().ToString();

            Note note = new Note
            {
                FileType = DAO.Models.Base.FileType.NOTE,
                Title = "Note's title",
                Content = "Note's content",
                OwnerID = ownerId,
                CreatedDateTime = DateTime.Now,
                IsShared = true,
                IsSharedForUsers = true
            };

            await databaseContext.Notes.AddAsync(note);

            ShareForUser shareForUser = new ShareForUser
            {
                CreatedDateTime = DateTime.Now,
                FileID = note.ID,
                OwnerID = ownerId,
                SharedForUserID = userId,
                ExpirationDateTime = DateTime.Now.AddDays(-1)
            };

            await databaseContext.ShareForUsers.AddAsync(shareForUser);

            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Notes
                .Any(_ => _.ID == note.ID));

            Assert.True(databaseContext.ShareForUsers
                .Any(_ => _.ID == shareForUser.ID));

            StatusCode<NoteOut> status = await notesService.GetByIdAndUser(note.ID, ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status404NotFound, status.Code);
        }

        [Fact]
        public async void Returns_NotFoundObjectResult404_when_NoteNotExists()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Note_to_NoteOut());
            });
            IMapper mapper = mapperConfiguration.CreateMapper();

            INotesService notesService = new NotesService(databaseContext, mapper);

            StatusCode<NoteOut> status = await notesService.GetByIdAndUser(Guid.NewGuid(), ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status404NotFound, status.Code);
        }

        [Fact]
        public async void Returns_NotFoundObjectResult404_when_NoteNotBelongsToLoggedUser()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Note_to_NoteOut());
            });
            IMapper mapper = mapperConfiguration.CreateMapper();

            INotesService notesService = new NotesService(databaseContext, mapper);

            Note note = new Note
            {
                FileType = DAO.Models.Base.FileType.NOTE,
                Title = "Note's title",
                Content = "Note's content",
                OwnerID = Guid.NewGuid().ToString(),
                CreatedDateTime = DateTime.Now
            };

            await databaseContext.Notes.AddAsync(note);
            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Notes.Any(_ => _.ID == note.ID));

            StatusCode<NoteOut> status = await notesService.GetByIdAndUser(note.ID, ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status404NotFound, status.Code);
        }
    }

    public class NotesServiceTest_PatchByIdAndFilePatchAndUser
    {
        private readonly static string ADMIN_USERNAME = "admin@admin.com";

        [Fact]
        public async void Returns_OkObjectResult200AndPatchedNoteOut_when_ChangedTitle()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Note_to_NoteOut());
                conf.AddProfile(new JsonPatchDocument_NotePatch_Mapper());
            });
            IMapper mapper = mapperConfiguration.CreateMapper();

            INotesService notesService = new NotesService(databaseContext, mapper);

            string userId = (await databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == ADMIN_USERNAME))?
                .Id;

            Note noteToChange = new Note
            {
                FileType = DAO.Models.Base.FileType.NOTE,
                Title = "Note's title",
                Content = "Note's content",
                OwnerID = userId,
                CreatedDateTime = DateTime.Now
            };

            await databaseContext.Notes.AddAsync(noteToChange);
            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Notes.Any(_ => _.ID == noteToChange.ID));

            string newTitle = "Changed title";

            JsonPatchDocument<NotePatch> jsonPatchDocument = new JsonPatchDocument<NotePatch>();
            jsonPatchDocument.Add(_ => _.Title, newTitle);

            StatusCode<NoteOut> status = await notesService.PatchByIdAndNotePatchAndUser(noteToChange.ID, jsonPatchDocument, ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status200OK, status.Code);
            Assert.NotNull(status.Body);
            Assert.Equal(newTitle, status.Body.Title);
            Assert.NotNull(status.Body.LastModifiedDateTime);
        }

        [Fact]
        public async void Returns_OkObjectResult200AndPatchedNoteOut_when_ChangedContent()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Note_to_NoteOut());
                conf.AddProfile(new JsonPatchDocument_NotePatch_Mapper());
            });
            IMapper mapper = mapperConfiguration.CreateMapper();

            INotesService notesService = new NotesService(databaseContext, mapper);

            string userId = (await databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == ADMIN_USERNAME))?
                .Id;

            Note noteToChange = new Note
            {
                FileType = DAO.Models.Base.FileType.NOTE,
                Title = "Note's title",
                Content = "Note's content",
                OwnerID = userId,
                CreatedDateTime = DateTime.Now
            };

            await databaseContext.Notes.AddAsync(noteToChange);
            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Notes.Any(_ => _.ID == noteToChange.ID));

            string newContent = "Changed content";

            JsonPatchDocument<NotePatch> jsonPatchDocument = new JsonPatchDocument<NotePatch>();
            jsonPatchDocument.Add(_ => _.Content, newContent);

            StatusCode<NoteOut> status = await notesService.PatchByIdAndNotePatchAndUser(noteToChange.ID, jsonPatchDocument, ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status200OK, status.Code);
            Assert.NotNull(status.Body);
            Assert.Equal(newContent, status.Body.Content);
            Assert.NotNull(status.Body.LastModifiedDateTime);
        }

        [Fact]
        public async void Returns_NotFoundObjectResult404_when_NoteNotBelongsToLoggedUser()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Note_to_NoteOut());
                conf.AddProfile(new JsonPatchDocument_NotePatch_Mapper());
            });
            IMapper mapper = mapperConfiguration.CreateMapper();

            INotesService notesService = new NotesService(databaseContext, mapper);

            string userId = (await databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == ADMIN_USERNAME))?
                .Id;

            Note noteToChange = new Note
            {
                FileType = DAO.Models.Base.FileType.NOTE,
                Title = "Note's title",
                Content = "Note's content",
                OwnerID = Guid.NewGuid().ToString(),
                CreatedDateTime = DateTime.Now
            };

            await databaseContext.Notes.AddAsync(noteToChange);
            await databaseContext.SaveChangesAsync();

            Assert.True(databaseContext.Notes.Any(_ => _.ID == noteToChange.ID));

            string newContent = "Changed content";

            JsonPatchDocument<NotePatch> jsonPatchDocument = new JsonPatchDocument<NotePatch>();
            jsonPatchDocument.Add(_ => _.Content, newContent);

            StatusCode<NoteOut> status = await notesService.PatchByIdAndNotePatchAndUser(noteToChange.ID, jsonPatchDocument, ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status404NotFound, status.Code);
        }

        [Fact]
        public async void Returns_NotFoundObjectResult404_when_NoteNotExists()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Note_to_NoteOut());
                conf.AddProfile(new JsonPatchDocument_NotePatch_Mapper());
            });
            IMapper mapper = mapperConfiguration.CreateMapper();

            INotesService notesService = new NotesService(databaseContext, mapper);

            string userId = (await databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == ADMIN_USERNAME))?
                .Id;

            string newContent = "Changed content";

            JsonPatchDocument<NotePatch> jsonPatchDocument = new JsonPatchDocument<NotePatch>();
            jsonPatchDocument.Add(_ => _.Content, newContent);

            StatusCode<NoteOut> status = await notesService.PatchByIdAndNotePatchAndUser(Guid.NewGuid(), jsonPatchDocument, ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status404NotFound, status.Code);
        }
    }

    public class NotesServiceTest_PostNoteByUser
    {
        private readonly static string ADMIN_USERNAME = "admin@admin.com";

        [Fact]
        public async void Returns_OkObjectResult200AndCreatedNote_when_NoteCreated()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Note_to_NoteOut());
            });
            IMapper mapper = mapperConfiguration.CreateMapper();

            INotesService notesService = new NotesService(databaseContext, mapper);

            NotePost newNotePost = new NotePost
            {
                Title = "Note's title",
                Content = "Note's content"
            };

            StatusCode<NoteOut> status = await notesService.PostNoteByUser(newNotePost, ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status200OK, status.Code);
            Assert.NotNull(status.Body);
            Assert.Equal(newNotePost.Title, status.Body.Title);
            Assert.Equal(newNotePost.Content, status.Body.Content);
            Assert.Equal(DAO.Models.Base.FileType.NOTE, status.Body.FileType);
            Assert.True(databaseContext.Notes.AnyAsync(_ => _.ID == status.Body.ID).Result);
        }
    }
}
