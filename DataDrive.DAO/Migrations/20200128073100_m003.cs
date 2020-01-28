using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataDrive.DAO.Migrations
{
    public partial class m003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "102eb830-42ae-45c4-a32c-f6c49bf994da", "f43bf214-a574-4d9a-835b-5d50c364b2e4" });

            migrationBuilder.DeleteData(
                table: "SystemConfigs",
                keyColumn: "ID",
                keyValue: new Guid("775ff964-9840-4b41-81c6-493921dceeec"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f43bf214-a574-4d9a-835b-5d50c364b2e4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "102eb830-42ae-45c4-a32c-f6c49bf994da");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfReports",
                table: "ResourceAbstracts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b55edec3-ab75-48fb-8a8d-b73e889527bc", "fbdec2c7-b5e6-45a4-95b2-93e60a7d021d", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TotalDiskSpace", "TwoFactorEnabled", "UsedDiskSpace", "UserName" },
                values: new object[] { "bb210b17-7815-4bc1-962b-5d8e21c8d7bd", 0, "0604d727-f3b8-4081-ac91-e8e3ed74b852", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEIJvg6szm+yVszEQRMxA4jJu36FxzLyKFUaUDxwfy0lWs8IDrV7EgZcKDRAjWH0sdQ==", null, false, "ed1f3b9f-4930-46ec-8c55-3e1343b0d867", 18446744073709551615m, false, 0m, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "SystemConfigs",
                columns: new[] { "ID", "TotalDiskSpaceForNewUser" },
                values: new object[] { new Guid("59cb4fe9-45a0-4de3-ae9b-74d17a49e0c8"), 1000000m });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "bb210b17-7815-4bc1-962b-5d8e21c8d7bd", "b55edec3-ab75-48fb-8a8d-b73e889527bc" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "bb210b17-7815-4bc1-962b-5d8e21c8d7bd", "b55edec3-ab75-48fb-8a8d-b73e889527bc" });

            migrationBuilder.DeleteData(
                table: "SystemConfigs",
                keyColumn: "ID",
                keyValue: new Guid("59cb4fe9-45a0-4de3-ae9b-74d17a49e0c8"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b55edec3-ab75-48fb-8a8d-b73e889527bc");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bb210b17-7815-4bc1-962b-5d8e21c8d7bd");

            migrationBuilder.DropColumn(
                name: "NumberOfReports",
                table: "ResourceAbstracts");

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
    }
}
