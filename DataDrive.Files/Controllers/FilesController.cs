using DataDrive.DAO.Helpers.Communication;
using DataDrive.DAO.Models;
using DataDrive.Files.Models;
using DataDrive.Files.Models.In;
using DataDrive.Files.Models.Out;
using DataDrive.Files.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataDrive.Files.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FilesController : Controller
    {
        private readonly IFileService _fileService;
        private readonly UserManager<ApplicationUser> _userManager;

        public FilesController(IFileService fileService, UserManager<ApplicationUser> userManager)
        {
            _fileService = fileService;
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid id)
        {
            StatusCode<FileOut> status = await _fileService.GetByIdAndUser(id, _userManager.GetUserName(User));

            if (status.Code == StatusCodes.Status404NotFound)
            {
                return NotFound($"File {id} not found");
            }

            return Ok(status.Body);
        }

        [HttpGet("fromDirectory/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetFromDirectory(Guid? id)
        {
            StatusCode<DirectoryOut> status = await _fileService.GetDirectoryByIdAndUser(id, _userManager.GetUserName(User));

            if (status.Code == StatusCodes.Status404NotFound)
            {
                return NotFound($"Directory {id} not found");
            }

            return Ok(status.Body);
        }

        [HttpGet("fromRoot")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetFromRoot()
        {
            StatusCode<DirectoryOut> status = await _fileService.GetDirectoryByIdAndUser(null, _userManager.GetUserName(User));

            if (status.Code == StatusCodes.Status404NotFound)
            {
                return NotFound($"Root directory not found");
            }

            return Ok(status.Body);
        }

        [HttpPost("createDirectory")]
        [Produces("application/json")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateDirectory([FromBody] DirectoryPost directoryPost)
        {
            StatusCode<DirectoryOut> result = await _fileService.CreateDirectoryByUser(directoryPost, _userManager.GetUserName(User));

            if (result.Code == StatusCodes.Status404NotFound)
            {
                return NotFound($"Directory {directoryPost.ParentDirectoryID} not exist");
            }

            return CreatedAtAction(nameof(GetFromDirectory), result.Body.ID);
        }

        [HttpGet("download/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [AllowAnonymous]
        public async Task<IActionResult> Download(Guid id)
        {
            StatusCode<DownloadFileInfo> status = await _fileService.DownloadByIdAndUser(id, _userManager.GetUserName(User));

            if (status.Code == StatusCodes.Status404NotFound)
            {
                return NotFound($"File {id} not found");
            }

            return File(status.Body.FileContent, status.Body.ContentType, status.Body.FileName);
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            StatusCode<DirectoryOut> result = await _fileService.DeleteByIdAndUser(id, _userManager.GetUserName(User));

            if (result.Code == StatusCodes.Status404NotFound)
            {
                return NotFound($"File {id} not found");
            }

            if (result.Code == StatusCodes.Status400BadRequest)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Body);
        }

        [HttpPatch("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<FilePatch> jsonPatch)
        {
            StatusCode<FileOut> status = await _fileService.PatchByIdAndFilePatchAndUser(id, jsonPatch, _userManager.GetUserName(User));

            if (status.Code == StatusCodes.Status404NotFound)
            {
                return NotFound($"File {id} not found");
            }

            return Ok(status.Body);
        }

        [HttpPost, DisableRequestSizeLimit]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Post([FromForm] FilePost filePost)
        {
            if (filePost.Files == null && Request.HasFormContentType)
            {
                filePost.Files = Request.Form.Files;
            }

            if (filePost.Files == null || filePost.Files.Count < 1)
            {
                return BadRequest("Something went wrong");
            }

            StatusCode<List<FileUploadResult>> status = await _fileService.PostByUser(filePost, _userManager
                .GetUserName(User));

            if (status.Code == StatusCodes.Status404NotFound)
            {
                return NotFound($"Directory {filePost.ParentDirectoryID} not found");
            }

            return Ok(status.Body);
        }
    }
}
