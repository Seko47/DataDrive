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
using static DataDrive.Files.Models.Out.UserDiskSpace;

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

        public SystemConfigOutputModel SystemConfigOut { get; set; }
        public List<ResourceAbstract> ReportedFiles { get; set; }

        [BindProperty]
        public SystemConfigInputModel SystemConfigInput { get; set; }

        public void OnGet()
        {
            SystemConfig systemConfig = _databaseContext.SystemConfigs
                .First();

            SystemConfigOut = new SystemConfigOutputModel
            {
                TotalDiskSpaceForNewUser = systemConfig.TotalDiskSpaceForNewUser
            };

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
        }

        public class SystemConfigOutputModel
        {
            private double totalDiskSpaceForNewUser;

            public double TotalDiskSpaceForNewUser
            {
                get => totalDiskSpaceForNewUser;

                set
                {
                    totalDiskSpaceForNewUser = value;

                    if (totalDiskSpaceForNewUser >= (ulong)Unit.TB)
                    {
                        DiskSpaceUnit = Unit.TB;
                        totalDiskSpaceForNewUser /= (ulong)Unit.TB;
                    }
                    else if (totalDiskSpaceForNewUser >= (ulong)Unit.GB)
                    {
                        DiskSpaceUnit = Unit.GB;
                        totalDiskSpaceForNewUser /= (ulong)Unit.GB;
                    }
                    else if (totalDiskSpaceForNewUser >= (ulong)Unit.MB)
                    {
                        DiskSpaceUnit = Unit.MB;
                        totalDiskSpaceForNewUser /= (ulong)Unit.MB;
                    }
                    else if (totalDiskSpaceForNewUser >= (ulong)Unit.kB)
                    {
                        DiskSpaceUnit = Unit.kB;
                        totalDiskSpaceForNewUser /= (ulong)Unit.kB;
                    }
                    else
                    {
                        DiskSpaceUnit = Unit.bytes;
                    }
                }
            }

            public Unit DiskSpaceUnit { get; set; }

            public string GetBytesWithUnit()
            {
                return $"{TotalDiskSpaceForNewUser} {DiskSpaceUnit.ToString()}";
            }
        }
    }
}
