using DataDrive.Files.Models.Out;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.Files.Services
{
    public interface IFileService
    {
        List<FileOut> GetAllFromRootByUser(string username);
    }
}
