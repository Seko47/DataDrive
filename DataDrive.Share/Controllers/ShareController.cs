using DataDrive.Share.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.Share.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShareController
    {
        private readonly IShareService _shareService;

        public ShareController(IShareService shareService)
        {
            _shareService = shareService;
        }
    }
}
