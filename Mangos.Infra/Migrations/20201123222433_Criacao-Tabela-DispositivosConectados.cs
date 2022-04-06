using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class CriacaoTabelaDispositivosConectados : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DispositivoConectado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Identificador = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DataHoraCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataHoraUltimoRefresh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataHoraExpiracaoRefreshToken = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sistema = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Expirado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispositivoConectado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DispositivoConectado_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DispositivoConectado_Identificador",
                table: "DispositivoConectado",
                column: "Identificador");

            migrationBuilder.CreateIndex(
                name: "IX_DispositivoConectado_UsuarioId",
                table: "DispositivoConectado",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DispositivoConectado");
        }
    }
}
