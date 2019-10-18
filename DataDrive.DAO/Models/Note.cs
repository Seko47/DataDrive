using DataDrive.DAO.Models.Base;

namespace DataDrive.DAO.Models
{
    public class Note : FileAbstract
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
