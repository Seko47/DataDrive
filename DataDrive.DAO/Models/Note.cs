using DataDrive.DAO.Models.Base;

namespace DataDrive.DAO.Models
{
    public class Note : ResourceAbstract
    {
        public string Title
        {
            get => Name;
            set => Name = value;
        }
        public string Content { get; set; }
    }
}
