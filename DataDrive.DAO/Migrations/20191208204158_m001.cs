using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataDrive.DAO.Migrations
{
    public partial class m001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MessageThreads",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageThreads", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ResourceAbstracts",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    LastModifiedDateTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OwnerID = table.Column<string>(nullable: true),
                    ParentDirectoryID = table.Column<Guid>(nullable: true),
                    ResourceType = table.Column<int>(nullable: false),
                    IsShared = table.Column<bool>(nullable: false),
                    IsSharedForEveryone = table.Column<bool>(nullable: false),
                    IsSharedForUsers = table.Column<bool>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceAbstracts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ResourceAbstracts_AspNetUsers_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResourceAbstracts_ResourceAbstracts_ParentDirectoryID",
                        column: x => x.ParentDirectoryID,
                        principalTable: "ResourceAbstracts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    SentDate = table.Column<DateTime>(nullable: false),
                    SendingUserID = table.Column<string>(nullable: true),
                    ThreadID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_SendingUserID",
                        column: x => x.SendingUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_MessageThreads_ThreadID",
                        column: x => x.ThreadID,
                        principalTable: "MessageThreads",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageThreadParticipants",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    ThreadID = table.Column<Guid>(nullable: false),
                    UserID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageThreadParticipants", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MessageThreadParticipants_MessageThreads_ThreadID",
                        column: x => x.ThreadID,
                        principalTable: "MessageThreads",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageThreadParticipants_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShareAbstracts",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    LastModifiedDateTime = table.Column<DateTime>(nullable: true),
                    ResourceID = table.Column<Guid>(nullable: false),
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
                        name: "FK_ShareAbstracts_AspNetUsers_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShareAbstracts_ResourceAbstracts_ResourceID",
                        column: x => x.ResourceID,
                        principalTable: "ResourceAbstracts",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ShareAbstracts_ResourceAbstracts_ResourceID1",
                        column: x => x.ResourceID,
                        principalTable: "ResourceAbstracts",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ShareAbstracts_AspNetUsers_SharedForUserID",
                        column: x => x.SharedForUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MessageReadStates",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    ReadDate = table.Column<DateTime>(nullable: false),
                    MessageID = table.Column<Guid>(nullable: false),
                    UserID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageReadStates", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MessageReadStates_Messages_MessageID",
                        column: x => x.MessageID,
                        principalTable: "Messages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageReadStates_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e02ec116-6182-4a2b-95f1-e0be8b1a068c", "fd7d147e-037b-4538-b9e1-30778600abbe", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "b9ccd7b9-6fce-4652-8b4c-e8c6b0046f83", 0, "ce7d11ee-5518-4ae3-a9d5-350c287455de", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEGd7Iy377V6C33JiTQRE/r1abNW8HNC+zbebEZSBLt+NM+P9xRCv1kW+qRQJ5Z5GFg==", null, false, "6125a779-984a-4b5c-8bc3-5e6c15e9703d", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "b9ccd7b9-6fce-4652-8b4c-e8c6b0046f83", "e02ec116-6182-4a2b-95f1-e0be8b1a068c" });

            migrationBuilder.CreateIndex(
                name: "IX_MessageReadStates_MessageID",
                table: "MessageReadStates",
                column: "MessageID");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReadStates_UserID",
                table: "MessageReadStates",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SendingUserID",
                table: "Messages",
                column: "SendingUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ThreadID",
                table: "Messages",
                column: "ThreadID");

            migrationBuilder.CreateIndex(
                name: "IX_MessageThreadParticipants_ThreadID",
                table: "MessageThreadParticipants",
                column: "ThreadID");

            migrationBuilder.CreateIndex(
                name: "IX_MessageThreadParticipants_UserID",
                table: "MessageThreadParticipants",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceAbstracts_OwnerID",
                table: "ResourceAbstracts",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceAbstracts_ParentDirectoryID",
                table: "ResourceAbstracts",
                column: "ParentDirectoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareAbstracts_OwnerID",
                table: "ShareAbstracts",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareAbstracts_ResourceID",
                table: "ShareAbstracts",
                column: "ResourceID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareAbstracts_SharedForUserID",
                table: "ShareAbstracts",
                column: "SharedForUserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageReadStates");

            migrationBuilder.DropTable(
                name: "MessageThreadParticipants");

            migrationBuilder.DropTable(
                name: "ShareAbstracts");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "ResourceAbstracts");

            migrationBuilder.DropTable(
                name: "MessageThreads");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "b9ccd7b9-6fce-4652-8b4c-e8c6b0046f83", "e02ec116-6182-4a2b-95f1-e0be8b1a068c" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e02ec116-6182-4a2b-95f1-e0be8b1a068c");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b9ccd7b9-6fce-4652-8b4c-e8c6b0046f83");
        }
    }
}
