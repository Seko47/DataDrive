using DataDrive.DAO.Context;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

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
    }
}
