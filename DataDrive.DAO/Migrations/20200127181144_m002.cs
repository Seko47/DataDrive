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
                keyValues: new object[] { "b9ccd7b9-6fce-4652-8b4c-e8c6b0046f83", "e02ec116-6182-4a2b-95f1-e0be8b1a068c" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e02ec116-6182-4a2b-95f1-e0be8b1a068c");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b9ccd7b9-6fce-4652-8b4c-e8c6b0046f83");

            migrationBuilder.AddColumn<int>(
                name: "TotalDiskSpace",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsedDiskSpace",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a2720423-3598-49a9-ab7b-d1b4340995ed", "94ce465e-fa39-422e-9f88-06ae3e9f1372", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TotalDiskSpace", "TwoFactorEnabled", "UsedDiskSpace", "UserName" },
                values: new object[] { "87a91a48-b1e8-4a70-8eae-32014c490575", 0, "eb0a4b1e-e96c-4689-bb81-da690c28260a", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEEh4rjbxc9iogngOJlnTGQ0DmVnqHtgmqXe8Ctq1yse5WvGIDACR3KoIn/XnFPLCCQ==", null, false, "6cf776c3-1707-40d8-85b2-d64408b0280e", 0, false, 0, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "87a91a48-b1e8-4a70-8eae-32014c490575", "a2720423-3598-49a9-ab7b-d1b4340995ed" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "87a91a48-b1e8-4a70-8eae-32014c490575", "a2720423-3598-49a9-ab7b-d1b4340995ed" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a2720423-3598-49a9-ab7b-d1b4340995ed");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "87a91a48-b1e8-4a70-8eae-32014c490575");

            migrationBuilder.DropColumn(
                name: "TotalDiskSpace",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UsedDiskSpace",
                table: "AspNetUsers");

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
        }
    }
}
