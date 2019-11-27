using DataDrive.DAO.Helpers.Communication;
using DataDrive.DAO.Models;
using DataDrive.Notes.Models.In;
using DataDrive.Notes.Models.Out;
using DataDrive.Notes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataDrive.Notes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : Controller
    {
        private readonly INotesService _notesService;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotesController(INotesService notesService, UserManager<ApplicationUser> userManager)
        {
            _notesService = notesService;
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid id)
        {
            StatusCode<NoteOut> status = await _notesService.GetByIdAndUser(id, _userManager.GetUserName(User));

            if (status.Code == StatusCodes.Status404NotFound)
            {
                return NotFound($"Note {id} not found");
            }

            return Ok(status.Body);
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get()
        {
            StatusCode<List<NoteOut>> status = await _notesService.GetAllByUser(_userManager.GetUserName(User));

            if (status.Code == StatusCodes.Status404NotFound)
            {
                return NotFound("Notes not found");
            }

            return Ok(status.Body);
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<NotePatch> jsonPatch)
        {
            throw new NotImplementedException();
        }

        [HttpPost, DisableRequestSizeLimit]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Post([FromBody] NotePost notePost)
        {
            throw new NotImplementedException();
        }
    }
}
