using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.Files.Models
{
    public class DownloadFileInfo
    {
        private readonly string _contentType;

        public string FileName { get; }
        public byte[] FileContent { get; }
        public string ContentType { get => _contentType; }

        public DownloadFileInfo(string fileName, byte[] fileContent)
        {
            FileName = fileName;
            FileContent = fileContent;

            //Get MIME type
            FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(FileName, out _contentType))
            {
                _contentType = "application/octet-stream";
            }
        }
    }
}
