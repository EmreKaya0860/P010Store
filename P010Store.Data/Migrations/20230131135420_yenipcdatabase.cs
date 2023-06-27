using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P010Store.Data.Migrations
{
    /// <inheritdoc />
    public partial class yenipcdatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2023, 1, 31, 16, 54, 20, 418, DateTimeKind.Local).AddTicks(6480));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2023, 1, 29, 10, 24, 20, 609, DateTimeKind.Local).AddTicks(659));
        }
    }
}
