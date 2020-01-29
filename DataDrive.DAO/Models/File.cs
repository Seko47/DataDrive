using DataDrive.DAO.Models.Base;

namespace DataDrive.DAO.Models
{
    public class File : ResourceAbstract
    {
        public string Path { get; set; }
        public ulong FileSizeBytes { get; set; }
    }
}
