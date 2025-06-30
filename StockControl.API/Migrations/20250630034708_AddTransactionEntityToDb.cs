using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockControl.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionEntityToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<double>(type: "REAL", nullable: false),
                    Tax = table.Column<double>(type: "REAL", nullable: true),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockOperations_TransactionId",
                table: "StockOperations",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockOperations_Transactions_TransactionId",
                table: "StockOperations",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockOperations_Transactions_TransactionId",
                table: "StockOperations");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_StockOperations_TransactionId",
                table: "StockOperations");
        }
    }
}
