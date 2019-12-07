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
            Assert.NotNull(status.Body.MessageThreadParticipants);
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
                        UserID=Guid.NewGuid().ToString()
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

    public class MessageServiceTest_GetThreadsByUser
    {
        private readonly static string ADMIN_USERNAME = "admin@admin.com";
        private readonly static string USER_USERNAME = "user@user.com";

        [Fact]
        public async void Returns_Status200OkAndListOfThreadOut_when_ThreadsExistAndHasMessages()
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

            Message message1 = new Message
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

            Message message2 = new Message
            {
                SendingUserID = adminId,
                SentDate = DateTime.Now,
                Content = "Second message's content",
                MessageReadStates = new List<MessageReadState>()
                {
                    new MessageReadState
                    {
                        ReadDate = DateTime.Now,
                        UserID = userId
                    }
                }
            };

            Message message3 = new Message
            {
                SendingUserID = adminId,
                SentDate = DateTime.Now,
                Content = "Third message's content",
                MessageReadStates = new List<MessageReadState>()
                {
                    new MessageReadState
                    {
                        ReadDate = DateTime.Now,
                        UserID = userId
                    }
                }
            };

            MessageThread thread1 = new MessageThread
            {
                Messages = new List<Message>()
                {
                    message1
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

            MessageThread thread2 = new MessageThread
            {
                Messages = new List<Message>()
                {
                    message2,
                    message3
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
                .AddRangeAsync(thread1, thread2);
            await databaseContext.SaveChangesAsync();

            StatusCode<List<ThreadOut>> status = await messageService.GetThreadsByUser(ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status200OK, status.Code);
            Assert.NotNull(status.Body);
            Assert.IsType<List<ThreadOut>>(status.Body);
            Assert.Equal(2, status.Body.Count);
            Assert.NotNull(status.Body[0].Messages);
            Assert.Single(status.Body[0].Messages);
            Assert.Equal(message3.Content, status.Body[0].Messages[0].Content);
            Assert.NotNull(status.Body[1].Messages);
            Assert.Single(status.Body[1].Messages);
            Assert.Equal(message1.Content, status.Body[1].Messages[0].Content);
            Assert.NotNull(status.Body[0].MessageThreadParticipants);
            Assert.NotNull(status.Body[1].MessageThreadParticipants);
            Assert.Equal(2, status.Body[0].MessageThreadParticipants.Count);
            Assert.Equal(2, status.Body[1].MessageThreadParticipants.Count);
        }

        [Fact]
        public async void Returns_Status404NotFound_when_ThreadsNotExist()
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

            StatusCode<List<ThreadOut>> status = await messageService.GetThreadsByUser(ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status404NotFound, status.Code);
        }

        [Fact]
        public async void Returns_Status404NotFound_when_ThreadsNotBelongToUser()
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

            Message message = new Message
            {
                SendingUserID = adminId,
                SentDate = DateTime.Now,
                Content = "Message's content",
                MessageReadStates = new List<MessageReadState>()
                {
                    new MessageReadState
                    {
                        ReadDate = DateTime.Now,
                        UserID = adminId
                    }
        }
            };

            MessageThread thread = new MessageThread
            {
                Messages = new List<Message>()
                {
                    message
                },
                MessageThreadParticipants = new List<MessageThreadParticipant>()
                {
                    new MessageThreadParticipant
                    {
                        UserID=adminId
                    }
                }
            };

            await databaseContext.MessageThreads
                .AddAsync(thread);
            await databaseContext.SaveChangesAsync();

            StatusCode<List<ThreadOut>> status = await messageService.GetThreadsByUser(USER_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status404NotFound, status.Code);
        }
    }

    public class MessageServiceTest_SendMessage
    {
        private readonly static string ADMIN_USERNAME = "admin@admin.com";
        private readonly static string USER_USERNAME = "user@user.com";

        [Fact]
        public async void Returns_StatusCode200AndMessageOut_when_MessageSentAndMessageThreadNotExists()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new MessageReadState_to_MessageReadStateOut());
                conf.AddProfile(new Message_to_MessageOut());
            });

            IMapper mapper = mapperConfiguration.CreateMapper();
            IMessageService messageService = new MessageService(databaseContext, mapper);

            string adminId = (await databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == ADMIN_USERNAME))?.Id;

            await DatabaseTestHelper.AddNewUser(USER_USERNAME, databaseContext);

            string userId = (await databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == USER_USERNAME))?.Id;

            MessagePost messagePost = new MessagePost
            {
                Content = "New message's content",
                ToUserUsername = USER_USERNAME
            };

            StatusCode<MessageOut> status = await messageService.SendMessage(messagePost, ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status200OK, status.Code);
            Assert.NotNull(status.Body);

            Message messageFromDatabase = await databaseContext.Messages.FirstOrDefaultAsync(_ => _.ID == status.Body.ID);
            Assert.NotNull(messageFromDatabase);
            Assert.True(await databaseContext.MessageThreads
                .AnyAsync(_ => _.Messages.Contains(messageFromDatabase)));

            List<MessageThreadParticipant> messageThreadParticipants = await databaseContext.MessageThreadParticipants
                .Where(_ => _.ThreadID == status.Body.ThreadID && (_.UserID == adminId || _.UserID == userId))
                .ToListAsync();
            Assert.Equal(2, messageThreadParticipants.Count);

            List<MessageReadState> messageReadStates = await databaseContext.MessageReadStates
                .Where(_ => _.MessageID == status.Body.ID)
                .ToListAsync();
            Assert.Single(messageReadStates);
            Assert.Contains(messageReadStates, _ => _.UserID == adminId);
        }

        [Fact]
        public async void Returns_StatusCode200AndMessageOut_when_MessageSentAndMessageThreadExists()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new MessageReadState_to_MessageReadStateOut());
                conf.AddProfile(new Message_to_MessageOut());
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

            MessageThread thread = new MessageThread
            {
                Messages = new List<Message>()
                {
                    messageFirst
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

            MessagePost messagePost = new MessagePost
            {
                Content = "New message's content",
                ToUserUsername = USER_USERNAME
            };

            StatusCode<MessageOut> status = await messageService.SendMessage(messagePost, ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status200OK, status.Code);
            Assert.NotNull(status.Body);

            Message messageFromDatabase = await databaseContext.Messages.FirstOrDefaultAsync(_ => _.ID == status.Body.ID);
            Assert.NotNull(messageFromDatabase);
            Assert.True(await databaseContext.MessageThreads
                .AnyAsync(_ => _.Messages.Contains(messageFromDatabase)));

            List<MessageThreadParticipant> messageThreadParticipants = await databaseContext.MessageThreadParticipants
                .Where(_ => _.ThreadID == status.Body.ThreadID && (_.UserID == adminId || _.UserID == userId))
                .ToListAsync();
            Assert.Equal(2, messageThreadParticipants.Count);

            List<MessageReadState> messageReadStates = await databaseContext.MessageReadStates
                .Where(_ => _.MessageID == status.Body.ID)
                .ToListAsync();
            Assert.Single(messageReadStates);
            Assert.Contains(messageReadStates, _ => _.UserID == adminId);
            Assert.Equal(thread.ID, status.Body.ThreadID);
        }

        [Fact]
        public async void Returns_StatusCode404_when_UserNotFound()
        {
            IDatabaseContext databaseContext = DatabaseTestHelper.GetContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new MessageReadState_to_MessageReadStateOut());
                conf.AddProfile(new Message_to_MessageOut());
            });

            IMapper mapper = mapperConfiguration.CreateMapper();
            IMessageService messageService = new MessageService(databaseContext, mapper);

            string adminId = (await databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == ADMIN_USERNAME))?.Id;

            MessagePost messagePost = new MessagePost
            {
                Content = "New message's content",
                ToUserUsername = USER_USERNAME
            };

            StatusCode<MessageOut> status = await messageService.SendMessage(messagePost, ADMIN_USERNAME);

            Assert.NotNull(status);
            Assert.Equal(StatusCodes.Status404NotFound, status.Code);
        }
    }
}
