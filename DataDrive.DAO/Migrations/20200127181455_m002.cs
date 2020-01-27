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
                values: new object[] { "d3cba9f8-a524-4324-84d5-12460b031eb0", "2f8aae5c-0066-4112-8568-92331eb6a85e", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TotalDiskSpace", "TwoFactorEnabled", "UsedDiskSpace", "UserName" },
                values: new object[] { "610eee7c-4fcd-425b-ac69-3fa5436dfad0", 0, "3a681e71-a0e8-470f-a8b1-6e74823539a3", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEGAqbhHDAPyoJUlcQZ0Tu/PgmOAEursiwmbPHAckf6te301SZXWk76HrEwOrNOFdDw==", null, false, "7ccc195d-0732-4f6a-a94c-c03e6f42f3c7", 2147483647, false, -2147483648, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "610eee7c-4fcd-425b-ac69-3fa5436dfad0", "d3cba9f8-a524-4324-84d5-12460b031eb0" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "610eee7c-4fcd-425b-ac69-3fa5436dfad0", "d3cba9f8-a524-4324-84d5-12460b031eb0" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d3cba9f8-a524-4324-84d5-12460b031eb0");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "610eee7c-4fcd-425b-ac69-3fa5436dfad0");

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
