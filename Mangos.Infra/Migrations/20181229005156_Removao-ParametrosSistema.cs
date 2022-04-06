using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class RemovaoParametrosSistema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParametrosSistema");

            migrationBuilder.AlterColumn<decimal>(
                name: "Valor",
                table: "MetaSaldo",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Valor",
                table: "MetaSaldo",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.CreateTable(
                name: "ParametrosSistema",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    DistanciaMaximaFornecedorDespesaRapida = table.Column<int>(nullable: false),
                    ExcluirBackupsDesatualizados = table.Column<bool>(nullable: false),
                    QuantidadeBackupsManter = table.Column<int>(nullable: false),
                    QuantidadeDiasBackupManter = table.Column<int>(nullable: false),
                    QuantidadeMesesBackupManter = table.Column<int>(nullable: false),
                    QuantidadeSegundasBackupManter = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParametrosSistema", x => x.Id);
                });
        }
    }
}
