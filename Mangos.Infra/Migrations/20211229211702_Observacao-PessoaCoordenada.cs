using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class ObservacaoPessoaCoordenada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descricao",
                table: "PessoaCoordenada",
                newName: "Observacao");

            migrationBuilder.AlterColumn<string>(
                name: "Observacao",
                table: "PessoaCoordenada",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.Sql("UPDATE PessoaCoordenada SET Observacao = NULL WHERE Observacao = 'Padrão'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Observacao",
                table: "PessoaCoordenada",
                newName: "Descricao");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "PessoaCoordenada",
                type: "nvarchar(100)",
                maxLength: 100,
                defaultValue: "Padrão",
                nullable: false);
        }
    }
}