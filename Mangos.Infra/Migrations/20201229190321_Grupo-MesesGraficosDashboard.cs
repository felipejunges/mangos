using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class GrupoMesesGraficosDashboard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MesesGraficosDashboard",
                table: "Grupo",
                type: "int",
                nullable: true);

            migrationBuilder.Sql("UPDATE Grupo SET MesesGraficosDashboard = 12");

            migrationBuilder.AlterColumn<string>("MesesGraficosDashboard", "Grupo", nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MesesGraficosDashboard",
                table: "Grupo");
        }
    }
}
