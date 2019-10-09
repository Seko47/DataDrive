using DataDrive.Files.Models.Out;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataDrive.Files.Services
{
    public interface IFileService
    {
        Task<List<FileOut>> GetAllFromRootByUser(string username);
    }
}
