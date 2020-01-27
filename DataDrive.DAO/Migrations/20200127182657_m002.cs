using System;
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

            migrationBuilder.AddColumn<decimal>(
                name: "TotalDiskSpace",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UsedDiskSpace",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "SystemConfigs",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    TotalDiskSpaceForNewUser = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemConfigs", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f43bf214-a574-4d9a-835b-5d50c364b2e4", "9f4440cc-f4b1-44f1-8a12-e68ea53360aa", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TotalDiskSpace", "TwoFactorEnabled", "UsedDiskSpace", "UserName" },
                values: new object[] { "102eb830-42ae-45c4-a32c-f6c49bf994da", 0, "3c8a7d5f-13d5-4ec6-9819-c265d687b20f", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEDsnb0htw8c/7/2fLh3kGsJrgmbp/Xn3dEqIPOmpF+qJHtNy3VKsVsBMUlA8eII4yw==", null, false, "7d08e759-5d5c-45f6-a13c-78e747ec343d", 18446744073709551615m, false, 0m, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "SystemConfigs",
                columns: new[] { "ID", "TotalDiskSpaceForNewUser" },
                values: new object[] { new Guid("775ff964-9840-4b41-81c6-493921dceeec"), 1000000m });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "102eb830-42ae-45c4-a32c-f6c49bf994da", "f43bf214-a574-4d9a-835b-5d50c364b2e4" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemConfigs");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "102eb830-42ae-45c4-a32c-f6c49bf994da", "f43bf214-a574-4d9a-835b-5d50c364b2e4" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f43bf214-a574-4d9a-835b-5d50c364b2e4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "102eb830-42ae-45c4-a32c-f6c49bf994da");

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
