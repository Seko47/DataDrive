using Microsoft.EntityFrameworkCore.Migrations;

namespace DataDrive.DAO.Migrations
{
    public partial class m006 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "FileAbstracts");

            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "FileAbstracts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a5a838b2-d3e2-4d6e-be03-fd9e8b4c1835", "9d8515b0-9565-496b-940a-40389aaf1c65", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "9c14745f-f6f6-49de-97fc-1d6488b83ad5", 0, "9a9ede34-6cc3-4701-93cf-d8c5bc4362ad", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEBHT+2tQukF86n/2f2WTK6rnZfcFsWtbbu9PfZff/CgjTYiL95uw2ZBcAxB9FystPg==", null, false, "b6008d8a-25fe-4f60-890f-e655aa9a4f07", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "9c14745f-f6f6-49de-97fc-1d6488b83ad5", "a5a838b2-d3e2-4d6e-be03-fd9e8b4c1835" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "9c14745f-f6f6-49de-97fc-1d6488b83ad5", "a5a838b2-d3e2-4d6e-be03-fd9e8b4c1835" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a5a838b2-d3e2-4d6e-be03-fd9e8b4c1835");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9c14745f-f6f6-49de-97fc-1d6488b83ad5");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "FileAbstracts");

            migrationBuilder.AddColumn<int>(
                name: "FileType",
                table: "FileAbstracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
