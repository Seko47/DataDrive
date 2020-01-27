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


        DbSet<ResourceAbstract> ResourceAbstracts { get; set; }
        DbSet<Directory> Directories { get; set; }
        DbSet<File> Files { get; set; }
        DbSet<Note> Notes { get; set; }

        DbSet<ShareAbstract> ShareAbstracts { get; set; }
        DbSet<ShareEveryone> ShareEveryones { get; set; }
        DbSet<ShareForUser> ShareForUsers { get; set; }

        DbSet<MessageThread> MessageThreads { get; set; }
        DbSet<MessageThreadParticipant> MessageThreadParticipants { get; set; }
        DbSet<Message> Messages { get; set; }
        DbSet<MessageReadState> MessageReadStates { get; set; }
        DbSet<SystemConfig> SystemConfigs { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
