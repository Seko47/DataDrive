using Microsoft.EntityFrameworkCore.Migrations;

namespace DataDrive.DAO.Migrations
{
    public partial class m002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cd2301ca-082d-4dec-b818-1a10d717b9cc", "1e1c3d32-0654-412f-a2f9-187f6a1b3d4c", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "182cde76-87c6-48ec-91df-36f7f17b9dce", 0, "9c52dedf-4582-467f-88e4-b6501fc5caae", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEGt4IxO5SXzHq/8uVHUUibg0vI+nBgEcwIvSlP1vaBo5OTyNG8NAlIyJGF9AqrsIiA==", null, false, "0ee26a11-2c78-424e-b26d-7d5e9bc14c61", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "182cde76-87c6-48ec-91df-36f7f17b9dce", "cd2301ca-082d-4dec-b818-1a10d717b9cc" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "182cde76-87c6-48ec-91df-36f7f17b9dce", "cd2301ca-082d-4dec-b818-1a10d717b9cc" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cd2301ca-082d-4dec-b818-1a10d717b9cc");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "182cde76-87c6-48ec-91df-36f7f17b9dce");
        }
    }
}
