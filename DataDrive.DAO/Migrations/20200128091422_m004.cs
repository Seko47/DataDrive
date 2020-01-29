using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataDrive.DAO.Migrations
{
    public partial class m004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<decimal>(
                name: "FileSizeBytes",
                table: "ResourceAbstracts",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8654c5e8-55ce-41b5-a096-296de5ec2bfd", "d6e7631d-f0fd-4cdf-9a05-235bb83ce748", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TotalDiskSpace", "TwoFactorEnabled", "UsedDiskSpace", "UserName" },
                values: new object[] { "f9059197-202e-41c5-b8e4-0f3f7b635646", 0, "76895db3-8d36-44ff-9302-31cd7ae9d677", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEDfpNpCeSHomYfjVfongxYtkBfOnFf+F9LTc06UJ9+2CZT47JDZsEUZTWCdlIpao9w==", null, false, "2dbaf588-f5c7-4a0e-b4c1-3eb98f1e8959", 18446744073709551615m, false, 0m, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "SystemConfigs",
                columns: new[] { "ID", "TotalDiskSpaceForNewUser" },
                values: new object[] { new Guid("c1622fc6-6430-4742-b9ef-3f95618fcd64"), 1000000m });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "f9059197-202e-41c5-b8e4-0f3f7b635646", "8654c5e8-55ce-41b5-a096-296de5ec2bfd" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "f9059197-202e-41c5-b8e4-0f3f7b635646", "8654c5e8-55ce-41b5-a096-296de5ec2bfd" });

            migrationBuilder.DeleteData(
                table: "SystemConfigs",
                keyColumn: "ID",
                keyValue: new Guid("c1622fc6-6430-4742-b9ef-3f95618fcd64"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8654c5e8-55ce-41b5-a096-296de5ec2bfd");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f9059197-202e-41c5-b8e4-0f3f7b635646");

            migrationBuilder.DropColumn(
                name: "FileSizeBytes",
                table: "ResourceAbstracts");

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
    }
}
