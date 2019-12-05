using DataDrive.DAO.Helpers.Communication;
using DataDrive.Messages.Models.In;
using DataDrive.Messages.Models.Out;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataDrive.Messages.Services
{
    public interface IMessageService
    {
        Task<StatusCode<List<ThreadOut>>> GetThreadsByUser(string username);
        Task<StatusCode<ThreadOut>> GetMessagesByThreadAndFilterAndUser(Guid threadId, MessageFilter messageFilter, string username);
        Task<StatusCode<MessageOut>> SendMessage(MessagePost messagePost, string senderUsername);
    }
}
