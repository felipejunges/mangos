using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class LancamentoFixoDataUltimoMesGerado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataUltimoVencimento",
                newName: "DataUltimoMesGerado",
                table: "LancamentoFixo");

            migrationBuilder.Sql("UPDATE LancamentoFixo SET DataUltimoMesGerado =DATEADD(DAY, -1 * (DATEPART(DAY, DataUltimoMesGerado) - 1), DataUltimoMesGerado)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataUltimoMesGerado",
                newName: "DataUltimoVencimento",
                table: "LancamentoFixo");
        }
    }
}
