using Microsoft.EntityFrameworkCore.Migrations;

namespace InvoiceTemplateUpload.Main.Migrations
{
    public partial class accountName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountName",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountName",
                table: "Invoices");
        }
    }
}
