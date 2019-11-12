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
                keyValues: new object[] { "db897393-2002-4e3c-8608-fbe1e7413466", "cab36395-5d43-401b-a325-db8d8e1c2628" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cab36395-5d43-401b-a325-db8d8e1c2628");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "db897393-2002-4e3c-8608-fbe1e7413466");

            migrationBuilder.AddColumn<bool>(
                name: "IsShared",
                table: "FileAbstracts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSharedForEveryone",
                table: "FileAbstracts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSharedForUsers",
                table: "FileAbstracts",
                nullable: false,
                defaultValue: false);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "IsShared",
                table: "FileAbstracts");

            migrationBuilder.DropColumn(
                name: "IsSharedForEveryone",
                table: "FileAbstracts");

            migrationBuilder.DropColumn(
                name: "IsSharedForUsers",
                table: "FileAbstracts");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cab36395-5d43-401b-a325-db8d8e1c2628", "bb6dfa6c-76b1-42d3-bfce-5055f6437a1b", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "db897393-2002-4e3c-8608-fbe1e7413466", 0, "77089553-1cda-4cc1-bb3b-5c7bf7b0bd21", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEO7fRhI0T+01sTu5t5oniThI01bZvMPTwZoGe3OWlO/uyH2RkEssjazbUkVt9ASOLQ==", null, false, "8390b1ce-30d6-4aad-87ab-1a6b7e4c9adc", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "db897393-2002-4e3c-8608-fbe1e7413466", "cab36395-5d43-401b-a325-db8d8e1c2628" });
        }
    }
}
