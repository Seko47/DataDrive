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
        Task<FileOut> GetByIdAndUser(Guid id, string username);
        Task<Tuple<string, byte[], string>> DownloadByIdAndUser(Guid id, string username);
        Task<FileOut> DeleteByIdAndUser(Guid id, string username);
    }
}
