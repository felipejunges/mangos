using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class FkPessoaPessoaCoordenadaCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PessoaCoordenada_Pessoa_PessoaId",
                table: "PessoaCoordenada");

            migrationBuilder.AddForeignKey(
                name: "FK_PessoaCoordenada_Pessoa_PessoaId",
                table: "PessoaCoordenada",
                column: "PessoaId",
                principalTable: "Pessoa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PessoaCoordenada_Pessoa_PessoaId",
                table: "PessoaCoordenada");

            migrationBuilder.AddForeignKey(
                name: "FK_PessoaCoordenada_Pessoa_PessoaId",
                table: "PessoaCoordenada",
                column: "PessoaId",
                principalTable: "Pessoa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
