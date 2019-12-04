using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataDrive.DAO.Helpers.Communication;
using DataDrive.Messages.Models.In;
using DataDrive.Messages.Models.Out;

namespace DataDrive.Messages.Services
{
    public class MessageService : IMessageService
    {
        //List of last messages from specified thread
        public Task<StatusCode<ThreadOut>> GetMessagesByThreadAndFilterAndUser(Guid guid, MessageFilter messageFilter, string v)
        {
            throw new NotImplementedException();
        }

        //List of thread with last message and second user
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
