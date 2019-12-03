using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataDrive.DAO.Migrations
{
    public partial class m007 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShareAbstracts_FileAbstracts_FileID",
                table: "ShareAbstracts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareAbstracts_FileAbstracts_FileID1",
                table: "ShareAbstracts");

            migrationBuilder.DropIndex(
                name: "IX_ShareAbstracts_FileID",
                table: "ShareAbstracts");

            migrationBuilder.DropIndex(
                name: "IX_ShareAbstracts_FileID1",
                table: "ShareAbstracts");

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
                name: "FileID",
                table: "ShareAbstracts");

            migrationBuilder.AddColumn<Guid>(
                name: "ResourceID",
                table: "ShareAbstracts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9a88f6e9-fd77-4ce0-b56b-b474f6e90f89", "5e25a0b1-378b-400a-9c61-50806326f013", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "8be03a7f-bf8d-4044-9a45-5ea2680ca273", 0, "9efd6e5d-042c-44a0-adc5-9c94ec2055c3", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEHnnTR/IYFORJvN/LKrqOlASPNLFIl7K/Ul9tPutH3e4yVQD177rbOOSQgBg/r/xqg==", null, false, "c7cd8dce-53e1-49dc-a1e1-448270c49540", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "8be03a7f-bf8d-4044-9a45-5ea2680ca273", "9a88f6e9-fd77-4ce0-b56b-b474f6e90f89" });

            migrationBuilder.CreateIndex(
                name: "IX_ShareAbstracts_ResourceID",
                table: "ShareAbstracts",
                column: "ResourceID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShareAbstracts_ResourceID1",
                table: "ShareAbstracts",
                column: "ResourceID");

            migrationBuilder.AddForeignKey(
                name: "FK_ShareAbstracts_FileAbstracts_ResourceID",
                table: "ShareAbstracts",
                column: "ResourceID",
                principalTable: "FileAbstracts",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ShareAbstracts_FileAbstracts_ResourceID1",
                table: "ShareAbstracts",
                column: "ResourceID",
                principalTable: "FileAbstracts",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShareAbstracts_FileAbstracts_ResourceID",
                table: "ShareAbstracts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareAbstracts_FileAbstracts_ResourceID1",
                table: "ShareAbstracts");

            migrationBuilder.DropIndex(
                name: "IX_ShareAbstracts_ResourceID",
                table: "ShareAbstracts");

            migrationBuilder.DropIndex(
                name: "IX_ShareAbstracts_ResourceID1",
                table: "ShareAbstracts");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "8be03a7f-bf8d-4044-9a45-5ea2680ca273", "9a88f6e9-fd77-4ce0-b56b-b474f6e90f89" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a88f6e9-fd77-4ce0-b56b-b474f6e90f89");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8be03a7f-bf8d-4044-9a45-5ea2680ca273");

            migrationBuilder.DropColumn(
                name: "ResourceID",
                table: "ShareAbstracts");

            migrationBuilder.AddColumn<Guid>(
                name: "FileID",
                table: "ShareAbstracts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.CreateIndex(
                name: "IX_ShareAbstracts_FileID",
                table: "ShareAbstracts",
                column: "FileID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShareAbstracts_FileID1",
                table: "ShareAbstracts",
                column: "FileID");

            migrationBuilder.AddForeignKey(
                name: "FK_ShareAbstracts_FileAbstracts_FileID",
                table: "ShareAbstracts",
                column: "FileID",
                principalTable: "FileAbstracts",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ShareAbstracts_FileAbstracts_FileID1",
                table: "ShareAbstracts",
                column: "FileID",
                principalTable: "FileAbstracts",
                principalColumn: "ID");
        }
    }
}
