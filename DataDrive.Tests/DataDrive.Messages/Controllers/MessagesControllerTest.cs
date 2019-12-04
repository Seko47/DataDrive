using DataDrive.DAO.Helpers.Communication;
using DataDrive.Messages.Controllers;
using DataDrive.Messages.Models.In;
using DataDrive.Messages.Models.Out;
using DataDrive.Messages.Services;
using DataDrive.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataDrive.Tests.DataDrive.Messages.Controllers
{
    public class MessagesControllerTest_GetThreads
    {
        private readonly static string ADMIN_USERNAME = "admin@admin.com";

        [Fact]
        public async void Returns_OkObjectResult200_when_ThreadsFound()
        {
            StatusCode<List<ThreadOut>> status = new StatusCode<List<ThreadOut>>(StatusCodes.Status200OK);
            Mock<IMessageService> messageService = new Mock<IMessageService>();
            messageService.Setup(_ => _.GetThreadsByUser(It.IsAny<string>()))
                .Returns(Task.FromResult(status));

            MessagesController messagesController = new MessagesController(messageService.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));
            messagesController.Authenticate(ADMIN_USERNAME);

            IActionResult result = await messagesController.GetThreads();

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Returns_ListOfThreadOut_when_ThreadsFound()
        {
            List<ThreadOut> listThreadOut = new List<ThreadOut>()
            {
                new ThreadOut(),
                new ThreadOut()
            };

            StatusCode<List<ThreadOut>> status = new StatusCode<List<ThreadOut>>(StatusCodes.Status200OK, listThreadOut);
            Mock<IMessageService> messageService = new Mock<IMessageService>();
            messageService.Setup(_ => _.GetThreadsByUser(It.IsAny<string>()))
                .Returns(Task.FromResult(status));

            MessagesController messagesController = new MessagesController(messageService.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));
            messagesController.Authenticate(ADMIN_USERNAME);

            IActionResult result = await messagesController.GetThreads();

            Assert.NotNull(result);
            OkObjectResult okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.NotNull(okObjectResult.Value);
            Assert.IsType<List<ThreadOut>>(okObjectResult.Value);
            Assert.Equal(2, (okObjectResult.Value as List<ThreadOut>).Count);
        }

        [Fact]
        public async void Returns_NotFoundObjectResult404_when_ThreadsNotFound()
        {
            StatusCode<List<ThreadOut>> status = new StatusCode<List<ThreadOut>>(StatusCodes.Status404NotFound);
            Mock<IMessageService> messageService = new Mock<IMessageService>();
            messageService.Setup(_ => _.GetThreadsByUser(It.IsAny<string>()))
                .Returns(Task.FromResult(status));

            MessagesController messagesController = new MessagesController(messageService.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));
            messagesController.Authenticate(ADMIN_USERNAME);

            IActionResult result = await messagesController.GetThreads();

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }

    public class MessagesControllerTest_GetMessagesFromThread
    {
        private readonly static string ADMIN_USERNAME = "admin@admin.com";

        [Fact]
        public async void Returns_OkObjectResult200_when_ThreadExistsAndContainsMessages()
        {
            StatusCode<ThreadOut> status = new StatusCode<ThreadOut>(StatusCodes.Status200OK);
            Mock<IMessageService> messageService = new Mock<IMessageService>();
            messageService.Setup(_ => _.GetMessagesByThreadAndFilterAndUser(It.IsAny<Guid>(), It.IsAny<MessageFilter>(), It.IsAny<string>()))
                .Returns(Task.FromResult(status));

            MessagesController messagesController = new MessagesController(messageService.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));
            messagesController.Authenticate(ADMIN_USERNAME);

            MessageFilter messageFilter = new MessageFilter
            {
                NumberOfLastMessage = 5
            };

            IActionResult result = await messagesController.GetMessagesFromThread(Guid.NewGuid(), messageFilter);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Returns_ThreadOutWithMessages_when_ThreadExistsAndContainsMessages()
        {
            ThreadOut threadOut = new ThreadOut
            {
                ID = Guid.NewGuid(),
                Messages = new List<MessageOut>()
                {
                    new MessageOut
                    {
                        ID=Guid.NewGuid(),
                        SentDate = DateTime.Now
                    },
                    new MessageOut
                    {
                        ID=Guid.NewGuid(),
                        SentDate = DateTime.Now
                    }
                }
            };

            StatusCode<ThreadOut> status = new StatusCode<ThreadOut>(StatusCodes.Status200OK, threadOut);
            Mock<IMessageService> messageService = new Mock<IMessageService>();
            messageService.Setup(_ => _.GetMessagesByThreadAndFilterAndUser(It.IsAny<Guid>(), It.IsAny<MessageFilter>(), It.IsAny<string>()))
                .Returns(Task.FromResult(status));

            MessagesController messagesController = new MessagesController(messageService.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));
            messagesController.Authenticate(ADMIN_USERNAME);

            MessageFilter messageFilter = new MessageFilter
            {
                NumberOfLastMessage = 5
            };

            IActionResult result = await messagesController.GetMessagesFromThread(Guid.NewGuid(), messageFilter);

            Assert.NotNull(result);
            OkObjectResult okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.NotNull(okObjectResult.Value);
            Assert.IsType<ThreadOut>(okObjectResult.Value);
            Assert.Equal(2, (okObjectResult.Value as ThreadOut).Messages.Count);
        }

        [Fact]
        public async void Returns_NotFoundObjectResult404_when_ThreadOrMessagesNotFound()
        {
            StatusCode<ThreadOut> status = new StatusCode<ThreadOut>(StatusCodes.Status404NotFound);
            Mock<IMessageService> messageService = new Mock<IMessageService>();
            messageService.Setup(_ => _.GetMessagesByThreadAndFilterAndUser(It.IsAny<Guid>(), It.IsAny<MessageFilter>(), It.IsAny<string>()))
                .Returns(Task.FromResult(status));

            MessagesController messagesController = new MessagesController(messageService.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));
            messagesController.Authenticate(ADMIN_USERNAME);

            MessageFilter messageFilter = new MessageFilter
            {
                NumberOfLastMessage = 5
            };

            IActionResult result = await messagesController.GetMessagesFromThread(Guid.NewGuid(), messageFilter);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }

    public class MessagesControllerTest_SendMessage
    {
        private readonly static string ADMIN_USERNAME = "admin@admin.com";

        [Fact]
        public async void Returns_OkObjectResult200_when_MessageSend()
        {
            StatusCode<MessageOut> status = new StatusCode<MessageOut>(StatusCodes.Status200OK);
            Mock<IMessageService> messageService = new Mock<IMessageService>();
            messageService.Setup(_ => _.SendMessage(It.IsAny<MessagePost>(), It.IsAny<string>()))
                .Returns(Task.FromResult(status));

            MessagesController messagesController = new MessagesController(messageService.Object, UserManagerHelper.GetUserManager(ADMIN_USERNAME));
            messagesController.Authenticate(ADMIN_USERNAME);

            MessagePost messagePost = new MessagePost
            {
                ToUserUsername = ADMIN_USERNAME,
                Content = "Message's content"
            };

            IActionResult result = await messagesController.SendMessage(messagePost);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }
        //404 when receiver user not exists
    }
}
