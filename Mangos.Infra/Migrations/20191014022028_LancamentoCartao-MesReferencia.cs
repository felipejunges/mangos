using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class LancamentoCartaoMesReferencia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MesVencimento",
                table: "LancamentoCartao",
                newName: "MesReferencia");

            migrationBuilder.Sql("UPDATE LancamentoCartao SET MesReferencia = DATEADD(MONTH, -1, MesReferencia) WHERE CartaoCreditoId IN (1,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MesReferencia",
                table: "LancamentoCartao",
                newName: "MesVencimento");

            migrationBuilder.Sql("UPDATE LancamentoCartao SET MesReferencia = DATEADD(MONTH, 1, MesReferencia) WHERE CartaoCreditoId IN (1,2)");
        }
    }
}