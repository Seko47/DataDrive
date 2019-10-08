using DataDrive.DAO.Models;
using DataDrive.DAO.Models.Base;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DataDrive.DAO.Context
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IDatabaseContext
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<FileAbstract> FileAbstracts { get; set; }
        public DbSet<Directory> Directories { get; set; }
        public DbSet<File> Files { get; set; }
    }
}
