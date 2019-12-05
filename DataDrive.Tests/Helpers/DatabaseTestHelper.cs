using DataDrive.DAO.Context;
using DataDrive.DAO.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace DataDrive.Tests.Helpers
{
    public static class DatabaseTestHelper
    {
        public static IDatabaseContext GetContext()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, Options.Create(new OperationalStoreOptions()));
            applicationDbContext.Database.EnsureCreated();

            return applicationDbContext;
        }

        public static async Task AddNewUser(string email, IDatabaseContext databaseContext)
        {
            ApplicationUser user = new ApplicationUser()
            {
                Email = email,
                EmailConfirmed = true,
                NormalizedEmail = email.ToUpper(),
                NormalizedUserName = email.ToUpper(),
                UserName = email
            };

            await databaseContext.Users
                .AddAsync(user);

            await databaseContext.SaveChangesAsync();
        }
    }
}
