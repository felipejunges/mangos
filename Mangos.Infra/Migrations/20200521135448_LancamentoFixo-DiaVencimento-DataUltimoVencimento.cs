using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class LancamentoFixoDiaVencimentoDataUltimoVencimento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UltimoMesGerado",
                newName: "DataUltimoVencimento",
                table: "LancamentoFixo");

            migrationBuilder.Sql("UPDATE LancamentoFixo SET DiaVencimento = 1 WHERE DiaVencimento IS NULL");
            migrationBuilder.Sql("UPDATE LancamentoFixo SET DataUltimoVencimento = DATEADD(DAY, DiaVencimento - 1, DataUltimoVencimento)");

            migrationBuilder.AlterColumn<int>(
                name: "DiaVencimento",
                table: "LancamentoFixo",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataUltimoVencimento",
                newName: "UltimoMesGerado",
                table: "LancamentoFixo");

            migrationBuilder.AlterColumn<int>(
                name: "DiaVencimento",
                table: "LancamentoFixo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}