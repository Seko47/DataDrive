using Microsoft.EntityFrameworkCore.Migrations;

namespace DataDrive.DAO.Migrations
{
    public partial class m003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShareAbstracts_FileAbstracts_FileID",
                table: "ShareAbstracts");

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

            migrationBuilder.RenameIndex(
                name: "IX_ShareAbstracts_FileID",
                table: "ShareAbstracts",
                newName: "IX_ShareAbstracts_FileID1");

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

            migrationBuilder.CreateIndex(
                name: "IX_ShareAbstracts_FileID",
                table: "ShareAbstracts",
                column: "FileID",
                unique: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameIndex(
                name: "IX_ShareAbstracts_FileID1",
                table: "ShareAbstracts",
                newName: "IX_ShareAbstracts_FileID");

            migrationBuilder.AddColumn<bool>(
                name: "IsShared",
                table: "FileAbstracts",
                type: "bit",
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

            migrationBuilder.AddForeignKey(
                name: "FK_ShareAbstracts_FileAbstracts_FileID",
                table: "ShareAbstracts",
                column: "FileID",
                principalTable: "FileAbstracts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
