using Microsoft.EntityFrameworkCore.Migrations;

namespace DataDrive.DAO.Migrations
{
    public partial class m005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "93c95336-d73e-4f2b-8cfd-5c37ed619019", "3657a297-7bd6-4878-8572-bc8292bfb95e" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3657a297-7bd6-4878-8572-bc8292bfb95e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93c95336-d73e-4f2b-8cfd-5c37ed619019");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "080912ac-2e07-4790-a56c-4bc096cc80cc", "34c7f30c-020a-4aaf-b487-1899a52b8cd3", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "60b3fc15-7140-42fb-825c-44d286696b95", 0, "e0825c86-afcc-40ff-a01b-0c6c31a9b6f5", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAELD6iIODxaaEulCwaEDixoryR5eCW9IP1Jp9WlVqadhTVCxfHvTlSJrNFo2iqN+xmQ==", null, false, "7263ef88-f16f-4925-9d53-f2b5c0a0a330", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "60b3fc15-7140-42fb-825c-44d286696b95", "080912ac-2e07-4790-a56c-4bc096cc80cc" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "60b3fc15-7140-42fb-825c-44d286696b95", "080912ac-2e07-4790-a56c-4bc096cc80cc" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "080912ac-2e07-4790-a56c-4bc096cc80cc");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "60b3fc15-7140-42fb-825c-44d286696b95");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3657a297-7bd6-4878-8572-bc8292bfb95e", "17f45512-0b8d-483b-b8b2-f8f871620fd3", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "93c95336-d73e-4f2b-8cfd-5c37ed619019", 0, "ebb001db-e236-4bea-a2e9-7407cfb92c2a", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEOe/PRs1U4Rh/NaraeRawLsVtP79GeQq32/IuXnMDmmEggkl2Fdo0qaYWSIPdOY22A==", null, false, "ffd2a7d5-f06f-489e-9522-bab8dc260528", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "93c95336-d73e-4f2b-8cfd-5c37ed619019", "3657a297-7bd6-4878-8572-bc8292bfb95e" });
        }
    }
}
