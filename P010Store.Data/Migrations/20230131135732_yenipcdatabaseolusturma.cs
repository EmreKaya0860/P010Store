using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P010Store.Data.Migrations
{
    /// <inheritdoc />
    public partial class yenipcdatabaseolusturma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2023, 1, 31, 16, 57, 32, 219, DateTimeKind.Local).AddTicks(2934));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2023, 1, 31, 16, 54, 20, 418, DateTimeKind.Local).AddTicks(6480));
        }
    }
}
