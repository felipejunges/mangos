using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class CriacaoTabelaLogEventos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PessoaCoordenada_Pessoa_PessoaId",
                table: "PessoaCoordenada");

            migrationBuilder.CreateTable(
                name: "LogEventos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DataHora = table.Column<DateTime>(type: "datetime", nullable: false),
                    LogLevel = table.Column<int>(maxLength: 20, nullable: false),
                    Mensagem = table.Column<string>(type: "nvarchar(MAX)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEventos", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PessoaCoordenada_Pessoa_PessoaId",
                table: "PessoaCoordenada",
                column: "PessoaId",
                principalTable: "Pessoa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PessoaCoordenada_Pessoa_PessoaId",
                table: "PessoaCoordenada");

            migrationBuilder.DropTable(
                name: "LogEventos");

            migrationBuilder.AddForeignKey(
                name: "FK_PessoaCoordenada_Pessoa_PessoaId",
                table: "PessoaCoordenada",
                column: "PessoaId",
                principalTable: "Pessoa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
