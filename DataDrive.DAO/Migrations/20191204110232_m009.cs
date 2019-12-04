using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataDrive.DAO.Migrations
{
    public partial class m009 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "9ce2c7b9-f48b-4f39-9131-29048f5d3854", "30fbd748-a448-4263-8c39-3cb7af957701" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "30fbd748-a448-4263-8c39-3cb7af957701");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9ce2c7b9-f48b-4f39-9131-29048f5d3854");

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
                values: new object[] { "7981dcf4-d5d5-4e10-b2bc-52bff9ed49b5", "52def8d5-7d74-4044-afdd-11cb320e06e9", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "428fb828-5dfb-4876-8e97-854e55192e55", 0, "2136946b-8376-496f-ab08-f57e25b2c8ae", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAELMrfP1Pxi5T5a0IHHYe49+QnMlVPEDxIE9STHV0VUoMagxUY7Xzp/NTDtgwA07JZg==", null, false, "9291979f-bd1b-4ebb-a16c-14f94e4f370a", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "428fb828-5dfb-4876-8e97-854e55192e55", "7981dcf4-d5d5-4e10-b2bc-52bff9ed49b5" });

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageReadStates");

            migrationBuilder.DropTable(
                name: "MessageThreadParticipants");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "MessageThreads");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "428fb828-5dfb-4876-8e97-854e55192e55", "7981dcf4-d5d5-4e10-b2bc-52bff9ed49b5" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7981dcf4-d5d5-4e10-b2bc-52bff9ed49b5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "428fb828-5dfb-4876-8e97-854e55192e55");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "30fbd748-a448-4263-8c39-3cb7af957701", "23f4a296-bd36-4dae-9de7-fcd482caf259", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "9ce2c7b9-f48b-4f39-9131-29048f5d3854", 0, "5fe2559b-d3bf-46a3-bf2f-4d03cc276dfa", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAELvVH7ZXmAkpozJk+L1460s9swI/YXXxMtj919Spdefq34aWgX/Jrrvj/L/E53pWzw==", null, false, "5bc6d4e4-8103-4c20-8ed4-aedc521049dc", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "9ce2c7b9-f48b-4f39-9131-29048f5d3854", "30fbd748-a448-4263-8c39-3cb7af957701" });
        }
    }
}
