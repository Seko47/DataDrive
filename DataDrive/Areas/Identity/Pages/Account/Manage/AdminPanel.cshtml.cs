using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DataDrive.DAO.Context;
using DataDrive.DAO.Models;
using DataDrive.DAO.Models.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DataDrive.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "admin")]
    public class AdminPanelModel : PageModel
    {
        private readonly IDatabaseContext _databaseContext;

        public AdminPanelModel(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public SystemConfig SystemConfig { get; set; }
        public List<ResourceAbstract> ReportedFiles { get; set; }

        [BindProperty]
        public SystemConfigInputModel SystemConfigInput { get; set; }

        public void OnGet()
        {
            SystemConfig = _databaseContext.SystemConfigs
                .First();

            ReportedFiles = _databaseContext.ResourceAbstracts
                .Where(_ => _.NumberOfReports > 0)
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                SystemConfig config = _databaseContext.SystemConfigs
                    .First();

                config.TotalDiskSpaceForNewUser = SystemConfigInput.GetBytes();
                await _databaseContext.SaveChangesAsync();
            }

            return RedirectToPage("AdminPanel");
        }

        public class SystemConfigInputModel
        {
            [Required]
            [Display(Name = "Total disk space for new user")]
            [Range(0, ulong.MaxValue)]
            public ulong TotalDiskSpaceForNewUser { get; set; }

            [Required]
            [Display(Name = "Unit")]
            public Unit DiskSpaceUnit { get; set; }

            public ulong GetBytes()
            {
                return TotalDiskSpaceForNewUser * (ulong)DiskSpaceUnit;
            }

            public enum Unit : ulong
            {
                Bytes = 1,
                kB = 1000,
                MB = 1000000,
                GB = 1000000000,
                TB = 1000000000000
            }
        }
    }
}
