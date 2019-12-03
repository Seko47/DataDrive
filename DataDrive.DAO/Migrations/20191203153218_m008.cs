using Microsoft.EntityFrameworkCore.Migrations;

namespace DataDrive.DAO.Migrations
{
    public partial class m008 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileAbstracts_AspNetUsers_OwnerID",
                table: "FileAbstracts");

            migrationBuilder.DropForeignKey(
                name: "FK_FileAbstracts_FileAbstracts_ParentDirectoryID",
                table: "FileAbstracts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareAbstracts_FileAbstracts_ResourceID",
                table: "ShareAbstracts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareAbstracts_FileAbstracts_ResourceID1",
                table: "ShareAbstracts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileAbstracts",
                table: "FileAbstracts");

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

            migrationBuilder.RenameTable(
                name: "FileAbstracts",
                newName: "ResourceAbstracts");

            migrationBuilder.RenameIndex(
                name: "IX_FileAbstracts_ParentDirectoryID",
                table: "ResourceAbstracts",
                newName: "IX_ResourceAbstracts_ParentDirectoryID");

            migrationBuilder.RenameIndex(
                name: "IX_FileAbstracts_OwnerID",
                table: "ResourceAbstracts",
                newName: "IX_ResourceAbstracts_OwnerID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResourceAbstracts",
                table: "ResourceAbstracts",
                column: "ID");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceAbstracts_AspNetUsers_OwnerID",
                table: "ResourceAbstracts",
                column: "OwnerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceAbstracts_ResourceAbstracts_ParentDirectoryID",
                table: "ResourceAbstracts",
                column: "ParentDirectoryID",
                principalTable: "ResourceAbstracts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShareAbstracts_ResourceAbstracts_ResourceID",
                table: "ShareAbstracts",
                column: "ResourceID",
                principalTable: "ResourceAbstracts",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ShareAbstracts_ResourceAbstracts_ResourceID1",
                table: "ShareAbstracts",
                column: "ResourceID",
                principalTable: "ResourceAbstracts",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceAbstracts_AspNetUsers_OwnerID",
                table: "ResourceAbstracts");

            migrationBuilder.DropForeignKey(
                name: "FK_ResourceAbstracts_ResourceAbstracts_ParentDirectoryID",
                table: "ResourceAbstracts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareAbstracts_ResourceAbstracts_ResourceID",
                table: "ShareAbstracts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareAbstracts_ResourceAbstracts_ResourceID1",
                table: "ShareAbstracts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResourceAbstracts",
                table: "ResourceAbstracts");

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

            migrationBuilder.RenameTable(
                name: "ResourceAbstracts",
                newName: "FileAbstracts");

            migrationBuilder.RenameIndex(
                name: "IX_ResourceAbstracts_ParentDirectoryID",
                table: "FileAbstracts",
                newName: "IX_FileAbstracts_ParentDirectoryID");

            migrationBuilder.RenameIndex(
                name: "IX_ResourceAbstracts_OwnerID",
                table: "FileAbstracts",
                newName: "IX_FileAbstracts_OwnerID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileAbstracts",
                table: "FileAbstracts",
                column: "ID");

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

            migrationBuilder.AddForeignKey(
                name: "FK_FileAbstracts_AspNetUsers_OwnerID",
                table: "FileAbstracts",
                column: "OwnerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FileAbstracts_FileAbstracts_ParentDirectoryID",
                table: "FileAbstracts",
                column: "ParentDirectoryID",
                principalTable: "FileAbstracts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

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
    }
}
