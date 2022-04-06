using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class GrupoMesesAntecedenciaLancamentoFixo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MesesAntecedenciaGerar",
                table: "LancamentoFixo");

            migrationBuilder.AddColumn<int>(
                name: "MesesAntecedenciaGerarLancamento",
                table: "Grupo",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MesesAntecedenciaGerarLancamentoCartao",
                table: "Grupo",
                nullable: true);

            migrationBuilder.Sql("UPDATE Grupo SET MesesAntecedenciaGerarLancamento = 6");
            migrationBuilder.Sql("UPDATE Grupo SET MesesAntecedenciaGerarLancamentoCartao = 0");

            migrationBuilder.AlterColumn<string>("MesesAntecedenciaGerarLancamento", "Grupo", nullable: false);
            migrationBuilder.AlterColumn<string>("MesesAntecedenciaGerarLancamentoCartao", "Grupo", nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MesesAntecedenciaGerarLancamento",
                table: "Grupo");

            migrationBuilder.DropColumn(
                name: "MesesAntecedenciaGerarLancamentoCartao",
                table: "Grupo");

            migrationBuilder.AddColumn<int>(
                name: "MesesAntecedenciaGerar",
                table: "LancamentoFixo",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
