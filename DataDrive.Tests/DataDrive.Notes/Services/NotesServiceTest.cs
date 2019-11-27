using AutoMapper;
using DataDrive.DAO.Context;
using DataDrive.DAO.Helpers.Communication;
using DataDrive.DAO.Models;
using DataDrive.Notes.Models.Out;
using DataDrive.Notes.Services;
using DataDrive.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
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
                conf.AddProfile(new FileAbstract_to_NoteOut());
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
                conf.AddProfile(new FileAbstract_to_NoteOut());
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
                conf.AddProfile(new FileAbstract_to_NoteOut());
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            INotesService notesService = new NotesService(databaseContext, mapper);

            StatusCode<Guid> status = await notesService.DeleteByIdAndUser(Guid.NewGuid(), ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status404NotFound, status.Code);
        }
    }
}
