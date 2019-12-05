using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataDrive.DAO.Context;
using DataDrive.DAO.Helpers.Communication;
using DataDrive.DAO.Models;
using DataDrive.Messages.Models.In;
using DataDrive.Messages.Models.Out;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DataDrive.Messages.Services
{
    public class MessageService : IMessageService
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public MessageService(IDatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }

        public async Task<StatusCode<ThreadOut>> GetMessagesByThreadAndFilterAndUser(Guid threadId, MessageFilter messageFilter, string username)
        {
            string userId = (await _databaseContext.Users
                .FirstOrDefaultAsync(_ => _.UserName == username))?.Id;

            MessageThread messageThread = await _databaseContext.MessageThreads
                .Include(_ => _.Messages)
                .Include(_ => _.MessageThreadParticipants)
                .FirstOrDefaultAsync(_ => _.ID == threadId && _.MessageThreadParticipants.Any(_ => _.UserID == userId));

            if (messageThread == null || !messageThread.Messages.Any())
            {
                return new StatusCode<ThreadOut>(StatusCodes.Status404NotFound, $"Thread {threadId} not found");
            }

            if (messageFilter.NumberOfLastMessage < 1)
            {
                messageFilter.NumberOfLastMessage = 1;
            }

            messageThread.Messages = messageThread.Messages
                .OrderBy(_ => _.SentDate)
                .TakeLast(messageFilter.NumberOfLastMessage)
                .ToList();

            ThreadOut result = _mapper.Map<ThreadOut>(messageThread);

            return new StatusCode<ThreadOut>(StatusCodes.Status200OK, result);
        }

        public Task<StatusCode<List<ThreadOut>>> GetThreadsByUser(string username)
        {
            throw new NotImplementedException();
        }

        public Task<StatusCode<MessageOut>> SendMessage(MessagePost messagePost, string senderUsername)
        {
            throw new NotImplementedException();
        }
    }
}
