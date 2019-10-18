using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DataDrive.DAO.Migrations
{
    public partial class m001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileAbstracts",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    LastModifiedDateTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OwnerID = table.Column<string>(nullable: true),
                    ParentDirectoryID = table.Column<Guid>(nullable: true),
                    FileType = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileAbstracts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FileAbstracts_AspNetUsers_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FileAbstracts_FileAbstracts_ParentDirectoryID",
                        column: x => x.ParentDirectoryID,
                        principalTable: "FileAbstracts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShareAbstracts",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    LastModifiedDateTime = table.Column<DateTime>(nullable: true),
                    FileID = table.Column<Guid>(nullable: false),
                    OwnerID = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    Token = table.Column<string>(nullable: true),
                    ExpirationDateTime = table.Column<DateTime>(nullable: true),
                    DownloadLimit = table.Column<int>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    SharedForUserID = table.Column<string>(nullable: true),
                    ShareForUser_ExpirationDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareAbstracts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ShareAbstracts_FileAbstracts_FileID",
                        column: x => x.FileID,
                        principalTable: "FileAbstracts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShareAbstracts_AspNetUsers_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShareAbstracts_AspNetUsers_SharedForUserID",
                        column: x => x.SharedForUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7f38fa11-d81b-4cf9-94a8-e586659e6ce6", "8d91ea64-5b66-4454-9ede-fa2cf27199cf", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "3420aa37-72ad-4305-b59b-95ce51815ddc", 0, "65185923-2e82-40d4-8a1d-32395b5932f3", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEGcNkeb5d8cX92f+9h2JVRUFb/HENB43TGo/+tT3Cl6NJB5T/EKsrdOTE512jDMQCw==", null, false, "a8b5d54d-e4c3-4d61-9786-1ba43c52ad29", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "3420aa37-72ad-4305-b59b-95ce51815ddc", "7f38fa11-d81b-4cf9-94a8-e586659e6ce6" });

            migrationBuilder.CreateIndex(
                name: "IX_FileAbstracts_OwnerID",
                table: "FileAbstracts",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_FileAbstracts_ParentDirectoryID",
                table: "FileAbstracts",
                column: "ParentDirectoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareAbstracts_FileID",
                table: "ShareAbstracts",
                column: "FileID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareAbstracts_OwnerID",
                table: "ShareAbstracts",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareAbstracts_SharedForUserID",
                table: "ShareAbstracts",
                column: "SharedForUserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShareAbstracts");

            migrationBuilder.DropTable(
                name: "FileAbstracts");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "3420aa37-72ad-4305-b59b-95ce51815ddc", "7f38fa11-d81b-4cf9-94a8-e586659e6ce6" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7f38fa11-d81b-4cf9-94a8-e586659e6ce6");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3420aa37-72ad-4305-b59b-95ce51815ddc");
        }
    }
}
