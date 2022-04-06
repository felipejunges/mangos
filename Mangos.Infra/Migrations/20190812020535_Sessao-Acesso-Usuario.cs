using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class SessaoAcessoUsuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataHoraUltimaAtualizacaoSessao",
                table: "Usuario");

            migrationBuilder.CreateTable(
                name: "SessaoAcesso",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<int>(nullable: false),
                    Chave = table.Column<string>(maxLength: 100, nullable: false),
                    DataHoraCriacao = table.Column<DateTime>(nullable: false),
                    DataHoraAtualizacao = table.Column<DateTime>(nullable: false),
                    Persistente = table.Column<bool>(nullable: false),
                    IP = table.Column<string>(maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(maxLength: 255, nullable: true),
                    IsMobile = table.Column<bool>(nullable: false),
                    Logout = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessaoAcesso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessaoAcesso_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessaoAcesso_UsuarioId",
                table: "SessaoAcesso",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessaoAcesso");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataHoraUltimaAtualizacaoSessao",
                table: "Usuario",
                nullable: true);
        }
    }
}
