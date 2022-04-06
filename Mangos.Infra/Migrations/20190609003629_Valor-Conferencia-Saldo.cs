using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class ValorConferenciaSaldo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ValorConferenciaSaldo",
                table: "SaldoContaBancaria",
                type: "money",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorConferenciaSaldo",
                table: "SaldoContaBancaria");
        }
    }
}
