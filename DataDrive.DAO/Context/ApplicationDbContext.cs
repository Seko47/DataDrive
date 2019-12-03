using DataDrive.DAO.Models;
using DataDrive.DAO.Models.Base;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

namespace DataDrive.DAO.Context
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IDatabaseContext
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<ResourceAbstract> ResourceAbstracts { get; set; }
        public DbSet<Directory> Directories { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Note> Notes { get; set; }

        public DbSet<ShareAbstract> ShareAbstracts { get; set; }
        public DbSet<ShareEveryone> ShareEveryones { get; set; }
        public DbSet<ShareForUser> ShareForUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ResourceAbstract>()
                .HasOne<ShareEveryone>(_ => _.ShareEveryone)
                .WithOne(_ => _.Resource)
                .HasForeignKey<ShareEveryone>(_ => _.ResourceID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ShareForUser>()
                .HasOne<ResourceAbstract>(_ => _.Resource)
                .WithMany(_ => _.ShareForUsers)
                .HasForeignKey(_ => _.ResourceID)
                .OnDelete(DeleteBehavior.NoAction);

            Seed(builder);
        }

        private void Seed(ModelBuilder builder)
        {
            string adminRoleID = Guid.NewGuid().ToString();

            builder.Entity<IdentityRole>()
                .HasData(
                    new IdentityRole
                    {
                        Id = adminRoleID,
                        Name = "admin",
                        NormalizedName = "ADMIN"
                    }
                );

            string adminID = Guid.NewGuid().ToString();
            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();

            builder.Entity<ApplicationUser>()
                .HasData(
                new ApplicationUser
                {
                    Id = adminID,
                    Email = "admin@admin.com",
                    NormalizedEmail = "ADMIN@ADMIN.COM",
                    UserName = "admin@admin.com",
                    NormalizedUserName = "ADMIN@ADMIN.COM",
                    EmailConfirmed = true,
                    PasswordHash = passwordHasher.HashPassword(null, "zaq1@WSX")
                });

            builder.Entity<IdentityUserRole<string>>()
                .HasData(
                    new IdentityUserRole<string>
                    {
                        RoleId = adminRoleID,
                        UserId = adminID
                    }
                );
        }
    }
}
