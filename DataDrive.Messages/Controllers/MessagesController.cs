using DataDrive.DAO.Helpers.Communication;
using DataDrive.DAO.Models;
using DataDrive.Messages.Models.In;
using DataDrive.Messages.Models.Out;
using DataDrive.Messages.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataDrive.Messages.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessagesController(IMessageService messageService, UserManager<ApplicationUser> userManager)
        {
            _messageService = messageService;
            _userManager = userManager;
        }

        [HttpGet("/threads")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetThreads()
        {
            StatusCode<List<ThreadOut>> status = await _messageService.GetThreadsByUser(_userManager.GetUserName(User));

            if (status.Code == StatusCodes.Status404NotFound)
            {
                return NotFound(status.Message);
            }

            return Ok(status.Body);
        }


        [HttpGet("/threads/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetMessagesFromThread(Guid id, MessageFilter messageFilter)
        {
            StatusCode<ThreadOut> status = await _messageService.GetMessagesByThreadAndFilterAndUser(id, messageFilter, _userManager.GetUserName(User));

            if (status.Code == StatusCodes.Status404NotFound)
            {
                return NotFound(status.Message);
            }

            return Ok(status.Body);
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> SendMessage(MessagePost messagePost)
        {
            throw new NotImplementedException();
        }
    }
}
