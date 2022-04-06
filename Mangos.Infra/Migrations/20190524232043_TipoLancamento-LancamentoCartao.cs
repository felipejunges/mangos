using Mangos.Dominio.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class TipoLancamentoLancamentoCartao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoLancamento",
                table: "LancamentoCartao",
                nullable: false,
                defaultValue: TipoLancamentoCartao.Despesa);

            migrationBuilder.AddColumn<bool>(
                name: "GerarLancamentoFecharMes",
                table: "CartaoCredito",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoLancamento",
                table: "LancamentoCartao");

            migrationBuilder.DropColumn(
                name: "GerarLancamentoFecharMes",
                table: "CartaoCredito");
        }
    }
}
