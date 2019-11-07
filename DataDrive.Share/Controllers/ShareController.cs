using DataDrive.DAO.Models;
using DataDrive.Share.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public ShareController(IShareService shareService, UserManager<ApplicationUser> userManager)
        {
            _shareService = shareService;
            _userManager = userManager;
        }
        //TODO share controller methods
        //Get all files shared for logged user (ShareForUser.cs)

        //Get shared file by Token (ShareEveryone.cs)

        //Disable sharing file by file id

        //Post, share file for everyone by file id (generates Token if user not specified)

        //Post, share file for specified user by file id and username

    }
}
