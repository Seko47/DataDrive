using DataDrive.Files.Models.Out;
using DataDrive.Files.Services;
using Microsoft.AspNetCore.Authorization;
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
    }
}
