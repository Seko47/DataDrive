using Microsoft.EntityFrameworkCore.Migrations;

namespace DataDrive.DAO.Migrations
{
    public partial class m002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<bool>(
                name: "IsShared",
                table: "FileAbstracts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b2ae439f-8856-474b-9b84-960bdafd6b45", "27a24a63-07c4-4f64-ace2-bb7b73e48988", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "84dc0264-bb38-4e0e-9cc6-be84fb8c62d2", 0, "bb7e880a-2d9d-4a3e-a439-65cfc5af344f", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEFrnsQXfEIIWrgFV5w7wanMxbVQ2t6IsXPWKNjdNVNyqjzmnFwX56It35URI/CWc0Q==", null, false, "ad0a2700-d883-4835-ada2-b07d080ed013", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "84dc0264-bb38-4e0e-9cc6-be84fb8c62d2", "b2ae439f-8856-474b-9b84-960bdafd6b45" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "84dc0264-bb38-4e0e-9cc6-be84fb8c62d2", "b2ae439f-8856-474b-9b84-960bdafd6b45" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b2ae439f-8856-474b-9b84-960bdafd6b45");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "84dc0264-bb38-4e0e-9cc6-be84fb8c62d2");

            migrationBuilder.DropColumn(
                name: "IsShared",
                table: "FileAbstracts");

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
        }
    }
}
