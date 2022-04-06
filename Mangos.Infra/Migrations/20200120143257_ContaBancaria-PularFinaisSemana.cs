using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class ContaBancariaPularFinaisSemana : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PularFinaisSemanaLancamentoRapido",
                table: "ContaBancaria",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PularFinaisSemanaLancamentoRapido",
                table: "ContaBancaria");
        }
    }
}
