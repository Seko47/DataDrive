using DataDrive.DAO.Models;
using DataDrive.DAO.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DataDrive.DAO.Context
{
    public interface IDatabaseContext
    {
        DbSet<ApplicationUser> Users { get; set; }


        DbSet<FileAbstract> FileAbstracts { get; set; }
        DbSet<Directory> Directories { get; set; }
        DbSet<File> Files { get; set; }
        DbSet<Note> Notes { get; set; }

        DbSet<ShareAbstract> ShareAbstracts { get; set; }
        DbSet<ShareEveryone> ShareEveryones { get; set; }
        DbSet<ShareForUser> ShareForUsers { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
