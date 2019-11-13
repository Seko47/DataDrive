using DataDrive.DAO.Helpers.Communication;
using DataDrive.DAO.Models;
using DataDrive.Share.Models;
using DataDrive.Share.Models.In;
using DataDrive.Share.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DataDrive.Share.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShareController : Controller
    {
        private readonly IShareService _shareService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShareController(IShareService shareService, UserManager<ApplicationUser> userManager)
        {
            _shareService = shareService;
            _userManager = userManager;
        }

        [HttpGet("{token}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [AllowAnonymous]
        public async Task<IActionResult> GetShareByToken(string token)
        {
            StatusCode<ShareEveryoneOut> status = await _shareService.GetShareForEveryoneByToken(token);

            if (status.Code == StatusCodes.Status404NotFound)
            {
                return NotFound($"Token {token} not found");
            }

            if (status.Code == StatusCodes.Status401Unauthorized)
            {
                return Unauthorized($"Password required for token {token}");
            }

            return Ok(status.Body);
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [AllowAnonymous]
        public async Task<IActionResult> GetShareByTokenAndPassword(string token, string password)
        {
            StatusCode<ShareEveryoneOut> status = await _shareService.GetShareForEveryoneByTokenAndPassword(token, password);

            if (status.Code == StatusCodes.Status404NotFound)
            {
                return NotFound($"Token {token} not found");
            }

            if (status.Code == StatusCodes.Status401Unauthorized)
            {
                return Unauthorized($"Password for token {token} is wrong");
            }

            return Ok(status.Body);
        }

        [HttpPost("everyone/share")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ShareForEveryone([FromBody] ShareEveryoneIn shareEveryoneIn)
        {
            StatusCode<ShareEveryoneOut> status = await _shareService.ShareForEveryone(shareEveryoneIn.FileId, _userManager.GetUserName(User), shareEveryoneIn.Password, shareEveryoneIn.ExpirationDateTime, shareEveryoneIn.DownloadLimit);

            if (status.Code == StatusCodes.Status404NotFound)
            {
                return NotFound($"File {shareEveryoneIn.FileId} not found");
            }

            return Ok(status.Body);
        }

        [HttpDelete("everyone/{fileId}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CancelShareForEveryone(Guid fileId)
        {
            bool result = await _shareService.CancelSharingForEveryone(fileId, _userManager.GetUserName(User));

            if (!result)
            {
                return BadRequest("Something went wrong");
            }

            return Ok(new { Canceled = result });
        }


        //TODO share controller methods
        //Get all files shared for logged user (ShareForUser.cs)

        //Post, share file for everyone by file id (generates Token if user not specified)

        //Post, share file for specified user by file id and username

    }
}
