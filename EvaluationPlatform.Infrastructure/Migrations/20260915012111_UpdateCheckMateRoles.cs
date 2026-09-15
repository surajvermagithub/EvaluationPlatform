using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckMate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCheckMateRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Description" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Institute administrator" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedOn", "Description", "Name" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teacher who evaluates examination booklets", "Teacher" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 9, 11, 21, 31, 19, 715, DateTimeKind.Utc).AddTicks(2451));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Description" },
                values: new object[] { new DateTime(2026, 9, 11, 21, 31, 19, 715, DateTimeKind.Utc).AddTicks(2455), "Administrator with limited access" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedOn", "Description", "Name" },
                values: new object[] { new DateTime(2026, 9, 11, 21, 31, 19, 715, DateTimeKind.Utc).AddTicks(2457), "Regular user with basic access", "User" });
        }
    }
}
