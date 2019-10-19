using DataDrive.DAO.Models;
using DataDrive.Files.Models.In;
using DataDrive.Files.Models.Out;
using DataDrive.Files.Services;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> Get(Guid id)
        {
            FileOut file = await _fileService.GetByIdAndUser(id, _userManager.GetUserName(User));

            if (file == null)
            {
                return NotFound($"File {id} not found");
            }

            return Ok(file);
        }

        [HttpGet("fromDirectory/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetFromDirectory(Guid? id)
        {
            DirectoryOut directory = await _fileService.GetDirectoryByIdAndUser(id, _userManager.GetUserName(User));

            if (directory == null)
            {
                return NotFound($"Directory {id} not found");
            }

            return Ok(directory);
        }

        [HttpGet("fromRoot")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetFromRoot()
        {
            DirectoryOut directory = await _fileService.GetDirectoryByIdAndUser(null, _userManager.GetUserName(User));

            if (directory == null)
            {
                return NotFound($"Root directory not found");
            }

            return Ok(directory);
        }

        [HttpPost("createDirectory")]
        [Produces("application/json")]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateDirectory([FromBody] DirectoryPost directoryPost)
        {
            DirectoryOut directory = await _fileService.CreateDirectoryByUser(directoryPost, _userManager.GetUserName(User));

            if (directory == null)
            {
                return NotFound($"Directory {directoryPost.ParentDirectoryID} not exist");
            }

            return CreatedAtAction(nameof(GetFromDirectory), directory.ID);
        }

        [HttpGet("download/{id}")]
        [Produces("application/octet-stream")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Download(Guid id)
        {
            Tuple<string, byte[], string> tuple = await _fileService.DownloadByIdAndUser(id, _userManager.GetUserName(User));

            if (tuple == null)
            {
                return NotFound($"File {id} not found");
            }

            string fileName = tuple.Item1;
            byte[] content = tuple.Item2;
            string contentType = tuple.Item3;//"application/octet-stream";

            return File(content, contentType, fileName);
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            FileOut parentDirectory = await _fileService.DeleteByIdAndUser(id, _userManager.GetUserName(User));

            if (parentDirectory == null)
            {
                return NotFound($"Directory {id} not found");
            }

            return Ok(parentDirectory);
        }

        [HttpPatch("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<FilePatch> jsonPatch)
        {
            FileOut file = await _fileService.PatchByIdAndFilePatchAndUser(id, jsonPatch, _userManager.GetUserName(User));

            if (file == null)
            {
                return NotFound($"File {id} not found");
            }

            return Ok(file);
        }

        [HttpPost, DisableRequestSizeLimit]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Post([FromForm] FilePost filePost)
        {
            if (filePost.Files == null || filePost.Files.Count < 1)
            {
                return BadRequest("Something went wrong");
            }

            List<FileUploadResult> fileUploadResults = await _fileService.PostByUser(filePost, _userManager.GetUserName(User));

            if (fileUploadResults == null)
            {
                return NotFound($"Directory {filePost.ParentDirectoryID} not found");
            }

            return Ok(fileUploadResults);
        }
    }
}
