using DataDrive.Files.Models.In;
using DataDrive.Files.Models.Out;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataDrive.Files.Services
{
    public interface IFileService
    {
        Task<FileOut> GetByIdAndUser(Guid id, string username);
        Task<Tuple<string, byte[], string>> DownloadByIdAndUser(Guid id, string username);
        Task<DirectoryOut> DeleteByIdAndUser(Guid id, string username);
        Task<FileOut> PatchByIdAndFilePatchAndUser(Guid id, JsonPatchDocument<FilePatch> jsonPatchDocument, string username);
        Task<List<FileUploadResult>> PostByUser(FilePost filePost, string username);
        Task<DirectoryOut> GetDirectoryByIdAndUser(Guid? id, string username);
        Task<DirectoryOut> CreateDirectoryByUser(DirectoryPost directoryPost, string username);
    }
}
