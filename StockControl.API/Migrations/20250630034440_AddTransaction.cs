using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockControl.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "StockOperations");

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "StockOperations");

            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "StockOperations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "StockOperations");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "StockOperations",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Tax",
                table: "StockOperations",
                type: "REAL",
                nullable: true);
        }
    }
}
