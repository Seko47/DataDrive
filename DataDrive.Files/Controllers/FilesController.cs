using DataDrive.DAO.Models;
using DataDrive.Files.Models.In;
using DataDrive.Files.Models.Out;
using DataDrive.Files.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataDrive.Files.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FilesController : Controller
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get()
        {
            List<FileOut> files = await _fileService.GetAllFromRootByUser(User.Identity.Name);

            return Ok(files);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(Guid id)
        {
            FileOut file = await _fileService.GetByIdAndUser(id, User.Identity.Name);

            if (file == null)
            {
                return NotFound($"File {id} not found");
            }

            return Ok(file);
        }

        [HttpGet("download/{id}")]
        [Produces("application/octet-stream")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Download(Guid id)
        {
            Tuple<string, byte[], string> tuple = await _fileService.DownloadByIdAndUser(id, User.Identity.Name);

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
            FileOut parentDirectory = await _fileService.DeleteByIdAndUser(id, User.Identity.Name);

            if (parentDirectory == null)
            {
                return NotFound($"Directory {id} not found");
            }

            return Ok(parentDirectory);
        }

        [HttpPatch("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<FilePatch> jsonPatch)
        {
            FileOut file = await _fileService.PatchByIdAndFilePatchAndUser(id, jsonPatch, User.Identity.Name);

            if (file == null)
            {
                return NotFound($"File {id} not found");
            }

            return Ok(file);
        }
    }
}
