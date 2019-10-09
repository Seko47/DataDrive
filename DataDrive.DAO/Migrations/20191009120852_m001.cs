using System;
using Microsoft.EntityFrameworkCore.Migrations;

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
                    LastModifiedDateTime = table.Column<DateTime>(nullable: false),
                    OwnerID = table.Column<string>(nullable: true),
                    ParentDirectoryID = table.Column<Guid>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    File_Name = table.Column<string>(nullable: true),
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
                    LastModifiedDateTime = table.Column<DateTime>(nullable: false),
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
        }
    }
}
