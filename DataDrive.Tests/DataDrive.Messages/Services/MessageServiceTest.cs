using AutoMapper;
using DataDrive.DAO.Context;
using DataDrive.DAO.Helpers.Communication;
using DataDrive.DAO.Models;
using DataDrive.Messages.Models.In;
using DataDrive.Messages.Models.Out;
using DataDrive.Messages.Services;
using DataDrive.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace DataDrive.Tests.DataDrive.Messages.Services
{
    public class MessageServiceTest_GetMessagesByThreadAndFilterAndUser
    {
        private readonly static string ADMIN_USERNAME = "admin@admin.com";
        private readonly static string USER_USERNAME = "user@user.com";

        [Fact]
        public async void Returns_Status200OkAndThreadOut_when_ThreadExistsAndHasMessages()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new MessageThread_to_ThreadOut());
                conf.AddProfile(new MessageReadState_to_MessageReadStateOut());
                conf.AddProfile(new Message_to_MessageOut());
                conf.AddProfile(new MessageThreadParticipant_to_MessageThreadParticipantOut());
            });

            IMapper mapper = mapperConfiguration.CreateMapper();
            IMessageService messageService = new MessageService(databaseContext, mapper);

            string adminId = (await databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == ADMIN_USERNAME))?.Id;

            await DatabaseTestHelper.AddNewUser(USER_USERNAME, databaseContext);

            string userId = (await databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == USER_USERNAME))?.Id;

            Message messageFirst = new Message
            {
                SendingUserID = adminId,
                SentDate = DateTime.Now,
                Content = "First message's content",
                MessageReadStates = new List<MessageReadState>()
                {
                    new MessageReadState
                    {
                        ReadDate = DateTime.Now,
                        UserID = adminId
                    }
        }
            };

            Message messageLast = new Message
            {
                SendingUserID = adminId,
                SentDate = DateTime.Now,
                Content = "Last message's content",
                MessageReadStates = new List<MessageReadState>()
                {
                    new MessageReadState
                    {
                        ReadDate = DateTime.Now,
                        UserID = userId
                    }
                }
            };

            MessageThread thread = new MessageThread
            {
                Messages = new List<Message>()
                {
                    messageFirst,
                    messageLast
                },
                MessageThreadParticipants = new List<MessageThreadParticipant>()
                {
                    new MessageThreadParticipant
                    {
                        UserID=adminId
                    },
                    new MessageThreadParticipant
                    {
                        UserID=userId
                    }
                }
            };

            await databaseContext.MessageThreads
                .AddAsync(thread);
            await databaseContext.SaveChangesAsync();

            int numberOfMessageToGetFromThread = 1;
            MessageFilter messageFilter = new MessageFilter
            {
                NumberOfLastMessage = numberOfMessageToGetFromThread
            };

            StatusCode<ThreadOut> status = await messageService.GetMessagesByThreadAndFilterAndUser(thread.ID, messageFilter, ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status200OK, status.Code);
            Assert.NotNull(status.Body);
            Assert.IsType<ThreadOut>(status.Body);
            Assert.NotNull(status.Body.Messages);
            Assert.Equal(numberOfMessageToGetFromThread, status.Body.Messages.Count);
            Assert.Equal(messageLast.Content, status.Body.Messages[0].Content);
            Assert.NotEmpty(status.Body.MessageThreadParticipants);
            Assert.Equal(2, status.Body.MessageThreadParticipants.Count);
            Assert.NotNull(status.Body.Messages[0].MessageReadStates);
            Assert.Contains(status.Body.Messages[0].MessageReadStates, _ => _.UserId == adminId);
            Assert.Contains(databaseContext.MessageReadStates, _ => _.UserID == adminId && _.MessageID == status.Body.Messages[0].ID);
        }

        [Fact]
        public async void Returns_Status404NotFound_when_ThreadNotExists()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new MessageThread_to_ThreadOut());
                conf.AddProfile(new MessageReadState_to_MessageReadStateOut());
                conf.AddProfile(new Message_to_MessageOut());
                conf.AddProfile(new MessageThreadParticipant_to_MessageThreadParticipantOut());
            });

            IMapper mapper = mapperConfiguration.CreateMapper();
            IMessageService messageService = new MessageService(databaseContext, mapper);

            int numberOfMessageToGetFromThread = 1;
            MessageFilter messageFilter = new MessageFilter
            {
                NumberOfLastMessage = numberOfMessageToGetFromThread
            };

            StatusCode<ThreadOut> status = await messageService.GetMessagesByThreadAndFilterAndUser(Guid.NewGuid(), messageFilter, ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status404NotFound, status.Code);
        }

        [Fact]
        public async void Returns_Status404NotFound_when_ThreadNotBelongsToUser()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new MessageThread_to_ThreadOut());
                conf.AddProfile(new MessageReadState_to_MessageReadStateOut());
                conf.AddProfile(new Message_to_MessageOut());
                conf.AddProfile(new MessageThreadParticipant_to_MessageThreadParticipantOut());
            });

            IMapper mapper = mapperConfiguration.CreateMapper();
            IMessageService messageService = new MessageService(databaseContext, mapper);

            string adminId = (await databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == ADMIN_USERNAME))?.Id;

            await DatabaseTestHelper.AddNewUser(USER_USERNAME, databaseContext);

            string userId = (await databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == USER_USERNAME))?.Id;

            MessageReadState messageReadState = new MessageReadState
            {
                ReadDate = DateTime.Now,
                UserID = adminId
            };

            Message messageFirst = new Message
            {
                SendingUserID = adminId,
                SentDate = DateTime.Now,
                Content = "First message's content"
            };

            Message messageLast = new Message
            {
                SendingUserID = adminId,
                SentDate = DateTime.Now,
                Content = "Last message's content",
                MessageReadStates = new List<MessageReadState>()
                {
                    messageReadState
                }
            };

            MessageThread thread = new MessageThread
            {
                Messages = new List<Message>()
                {
                    messageFirst,
                    messageLast
                },
                MessageThreadParticipants = new List<MessageThreadParticipant>()
                {
                    new MessageThreadParticipant
                    {
                        UserID=adminId
                    },
                    new MessageThreadParticipant
                    {
                        UserID=Guid.NewGuid().ToString()
                    }
                }
            };

            await databaseContext.MessageThreads
                .AddAsync(thread);
            await databaseContext.SaveChangesAsync();

            int numberOfMessageToGetFromThread = 1;
            MessageFilter messageFilter = new MessageFilter
            {
                NumberOfLastMessage = numberOfMessageToGetFromThread
            };

            StatusCode<ThreadOut> status = await messageService.GetMessagesByThreadAndFilterAndUser(thread.ID, messageFilter, USER_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status404NotFound, status.Code);
        }
    }
}
