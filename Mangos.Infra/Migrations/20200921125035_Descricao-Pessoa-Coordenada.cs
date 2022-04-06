using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class DescricaoPessoaCoordenada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ordem",
                table: "PessoaCoordenada");

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "PessoaCoordenada",
                nullable: true);

            migrationBuilder.Sql("UPDATE PessoaCoordenada SET Descricao = 'Padrão'");

            migrationBuilder.AlterColumn<string>("Descricao", "PessoaCoordenada", nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "PessoaCoordenada");

            migrationBuilder.AddColumn<int>(
                name: "Ordem",
                table: "PessoaCoordenada",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
