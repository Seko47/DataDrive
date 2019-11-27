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
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            StatusCode<Guid> status = await _notesService.DeleteByIdAndUser(id, _userManager.GetUserName(User));

            if (status.Code == StatusCodes.Status404NotFound)
            {
                return NotFound($"Note {id} not found");
            }

            if (status.Code == StatusCodes.Status400BadRequest)
            {
                return BadRequest(status.Message);
            }

            return Ok(status.Body);
        }

        [HttpPatch("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<NotePatch> jsonPatch)
        {
            StatusCode<NoteOut> status = await _notesService.PatchByIdAndFilePatchAndUser(id, jsonPatch, _userManager.GetUserName(User));

            if (status.Code == StatusCodes.Status404NotFound)
            {
                return NotFound($"Note {id} not found");
            }

            return Ok(status.Body);
        }

        [HttpPost, DisableRequestSizeLimit]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Post([FromBody] NotePost notePost)
        {
            StatusCode<NoteOut> status = await _notesService.PostNoteByUser(notePost, _userManager.GetUserName(User));

            return Ok(status.Body);
        }
    }
}
