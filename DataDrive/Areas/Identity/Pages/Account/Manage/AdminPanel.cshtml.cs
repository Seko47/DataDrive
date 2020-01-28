using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DataDrive.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "admin")]
    public class AdminPanelModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
