﻿// <auto-generated />
using System;
using DataDrive.DAO.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataDrive.DAO.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataDrive.DAO.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalDiskSpace")
                        .HasColumnType("decimal(20,0)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<decimal>("UsedDiskSpace")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasData(
                        new
                        {
                            Id = "bb210b17-7815-4bc1-962b-5d8e21c8d7bd",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "0604d727-f3b8-4081-ac91-e8e3ed74b852",
                            Email = "admin@admin.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            NormalizedEmail = "ADMIN@ADMIN.COM",
                            NormalizedUserName = "ADMIN@ADMIN.COM",
                            PasswordHash = "AQAAAAEAACcQAAAAEIJvg6szm+yVszEQRMxA4jJu36FxzLyKFUaUDxwfy0lWs8IDrV7EgZcKDRAjWH0sdQ==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "ed1f3b9f-4930-46ec-8c55-3e1343b0d867",
                            TotalDiskSpace = 18446744073709551615m,
                            TwoFactorEnabled = false,
                            UsedDiskSpace = 0m,
                            UserName = "admin@admin.com"
                        });
                });

            modelBuilder.Entity("DataDrive.DAO.Models.Base.ResourceAbstract", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsShared")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSharedForEveryone")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSharedForUsers")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfReports")
                        .HasColumnType("int");

                    b.Property<string>("OwnerID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid?>("ParentDirectoryID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ResourceType")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("OwnerID");

                    b.HasIndex("ParentDirectoryID");

                    b.ToTable("ResourceAbstracts");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ResourceAbstract");
                });

            modelBuilder.Entity("DataDrive.DAO.Models.Base.ShareAbstract", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("OwnerID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("ResourceID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.HasIndex("OwnerID");

                    b.ToTable("ShareAbstracts");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ShareAbstract");
                });

            modelBuilder.Entity("DataDrive.DAO.Models.Message", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SendingUserID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("SentDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ThreadID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.HasIndex("SendingUserID");

                    b.HasIndex("ThreadID");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("DataDrive.DAO.Models.MessageReadState", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MessageID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ReadDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserID")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasIndex("MessageID");

                    b.HasIndex("UserID");

                    b.ToTable("MessageReadStates");
                });

            modelBuilder.Entity("DataDrive.DAO.Models.MessageThread", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.ToTable("MessageThreads");
                });

            modelBuilder.Entity("DataDrive.DAO.Models.MessageThreadParticipant", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ThreadID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserID")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasIndex("ThreadID");

                    b.HasIndex("UserID");

                    b.ToTable("MessageThreadParticipants");
                });

            modelBuilder.Entity("DataDrive.DAO.Models.SystemConfig", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("TotalDiskSpaceForNewUser")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("ID");

                    b.ToTable("SystemConfigs");

                    b.HasData(
                        new
                        {
                            ID = new Guid("59cb4fe9-45a0-4de3-ae9b-74d17a49e0c8"),
                            TotalDiskSpaceForNewUser = 1000000m
                        });
                });

            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.DeviceFlowCodes", b =>
                {
                    b.Property<string>("UserCode")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasMaxLength(50000);

                    b.Property<string>("DeviceCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<DateTime?>("Expiration")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("SubjectId")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("UserCode");

                    b.HasIndex("DeviceCode")
                        .IsUnique();

                    b.HasIndex("Expiration");

                    b.ToTable("DeviceCodes");
                });

            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.PersistedGrant", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasMaxLength(50000);

                    b.Property<DateTime?>("Expiration")
                        .HasColumnType("datetime2");

                    b.Property<string>("SubjectId")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Key");

                    b.HasIndex("Expiration");

                    b.HasIndex("SubjectId", "ClientId", "Type");

                    b.ToTable("PersistedGrants");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");

                    b.HasData(
                        new
                        {
                            Id = "b55edec3-ab75-48fb-8a8d-b73e889527bc",
                            ConcurrencyStamp = "fbdec2c7-b5e6-45a4-95b2-93e60a7d021d",
                            Name = "admin",
                            NormalizedName = "ADMIN"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");

                    b.HasData(
                        new
                        {
                            UserId = "bb210b17-7815-4bc1-962b-5d8e21c8d7bd",
                            RoleId = "b55edec3-ab75-48fb-8a8d-b73e889527bc"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("DataDrive.DAO.Models.Directory", b =>
                {
                    b.HasBaseType("DataDrive.DAO.Models.Base.ResourceAbstract");

                    b.HasDiscriminator().HasValue("Directory");
                });

            modelBuilder.Entity("DataDrive.DAO.Models.File", b =>
                {
                    b.HasBaseType("DataDrive.DAO.Models.Base.ResourceAbstract");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("File");
                });

            modelBuilder.Entity("DataDrive.DAO.Models.Note", b =>
                {
                    b.HasBaseType("DataDrive.DAO.Models.Base.ResourceAbstract");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Note");
                });

            modelBuilder.Entity("DataDrive.DAO.Models.ShareEveryone", b =>
                {
                    b.HasBaseType("DataDrive.DAO.Models.Base.ShareAbstract");

                    b.Property<int?>("DownloadLimit")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ExpirationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.HasIndex("ResourceID")
                        .IsUnique();

                    b.HasDiscriminator().HasValue("ShareEveryone");
                });

            modelBuilder.Entity("DataDrive.DAO.Models.ShareForUser", b =>
                {
                    b.HasBaseType("DataDrive.DAO.Models.Base.ShareAbstract");

                    b.Property<DateTime?>("ExpirationDateTime")
                        .HasColumnName("ShareForUser_ExpirationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SharedForUserID")
                        .HasColumnType("nvarchar(450)");

                    b.HasIndex("ResourceID")
                        .HasName("IX_ShareAbstracts_ResourceID1");

                    b.HasIndex("SharedForUserID");

                    b.HasDiscriminator().HasValue("ShareForUser");
                });

            modelBuilder.Entity("DataDrive.DAO.Models.Base.ResourceAbstract", b =>
                {
                    b.HasOne("DataDrive.DAO.Models.ApplicationUser", "Owner")
                        .WithMany("Files")
                        .HasForeignKey("OwnerID");

                    b.HasOne("DataDrive.DAO.Models.Directory", "ParentDirectory")
                        .WithMany("Files")
                        .HasForeignKey("ParentDirectoryID");
                });

            modelBuilder.Entity("DataDrive.DAO.Models.Base.ShareAbstract", b =>
                {
                    b.HasOne("DataDrive.DAO.Models.ApplicationUser", "Owner")
                        .WithMany("SharedOwn")
                        .HasForeignKey("OwnerID");
                });

            modelBuilder.Entity("DataDrive.DAO.Models.Message", b =>
                {
                    b.HasOne("DataDrive.DAO.Models.ApplicationUser", "SendingUser")
                        .WithMany()
                        .HasForeignKey("SendingUserID");

                    b.HasOne("DataDrive.DAO.Models.MessageThread", "Thread")
                        .WithMany("Messages")
                        .HasForeignKey("ThreadID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataDrive.DAO.Models.MessageReadState", b =>
                {
                    b.HasOne("DataDrive.DAO.Models.Message", "Message")
                        .WithMany("MessageReadStates")
                        .HasForeignKey("MessageID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataDrive.DAO.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("DataDrive.DAO.Models.MessageThreadParticipant", b =>
                {
                    b.HasOne("DataDrive.DAO.Models.MessageThread", "Thread")
                        .WithMany("MessageThreadParticipants")
                        .HasForeignKey("ThreadID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataDrive.DAO.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DataDrive.DAO.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DataDrive.DAO.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataDrive.DAO.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("DataDrive.DAO.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataDrive.DAO.Models.ShareEveryone", b =>
                {
                    b.HasOne("DataDrive.DAO.Models.Base.ResourceAbstract", "Resource")
                        .WithOne("ShareEveryone")
                        .HasForeignKey("DataDrive.DAO.Models.ShareEveryone", "ResourceID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("DataDrive.DAO.Models.ShareForUser", b =>
                {
                    b.HasOne("DataDrive.DAO.Models.Base.ResourceAbstract", "Resource")
                        .WithMany("ShareForUsers")
                        .HasForeignKey("ResourceID")
                        .HasConstraintName("FK_ShareAbstracts_ResourceAbstracts_ResourceID1")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DataDrive.DAO.Models.ApplicationUser", "SharedForUser")
                        .WithMany("SharedForUser")
                        .HasForeignKey("SharedForUserID");
                });
#pragma warning restore 612, 618
        }
    }
}
