using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class FlagRelatorioCartaoCredito : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ExibirDadosRelatorio",
                table: "CartaoCredito",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExibirDadosRelatorio",
                table: "CartaoCredito");
        }
    }
}
