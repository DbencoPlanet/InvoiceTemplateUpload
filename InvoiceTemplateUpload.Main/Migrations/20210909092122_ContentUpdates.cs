using Microsoft.EntityFrameworkCore.Migrations;

namespace InvoiceTemplateUpload.Main.Migrations
{
    public partial class ContentUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Due",
                table: "Invoices");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyPrefix",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyPrefix",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Invoices");

            migrationBuilder.AddColumn<decimal>(
                name: "Due",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
