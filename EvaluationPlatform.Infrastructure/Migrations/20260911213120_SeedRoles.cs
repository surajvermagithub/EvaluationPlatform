using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CheckMate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "Description", "IsActive", "IsDeleted", "Name", "UpdatedBy", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 9, 11, 21, 31, 19, 715, DateTimeKind.Utc).AddTicks(2451), "Super Administrator with full system access", true, false, "SuperAdmin", null, null },
                    { 2, null, new DateTime(2026, 9, 11, 21, 31, 19, 715, DateTimeKind.Utc).AddTicks(2455), "Administrator with limited access", true, false, "Admin", null, null },
                    { 3, null, new DateTime(2026, 9, 11, 21, 31, 19, 715, DateTimeKind.Utc).AddTicks(2457), "Regular user with basic access", true, false, "User", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
