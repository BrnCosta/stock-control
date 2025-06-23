using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockControl.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTaxStockOperation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Tax",
                table: "StockOperations",
                type: "REAL",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tax",
                table: "StockOperations");
        }
    }
}
