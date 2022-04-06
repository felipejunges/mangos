using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class CartaoCreditoOffsetReferenciaVencimento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessaoAcesso_Usuario_UsuarioId",
                table: "SessaoAcesso");

            migrationBuilder.AddColumn<int>(
                name: "OffsetReferenciaVencimento",
                table: "CartaoCredito",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SessaoAcesso_Chave",
                table: "SessaoAcesso",
                column: "Chave");

            migrationBuilder.AddForeignKey(
                name: "FK_SessaoAcesso_Usuario_UsuarioId",
                table: "SessaoAcesso",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessaoAcesso_Usuario_UsuarioId",
                table: "SessaoAcesso");

            migrationBuilder.DropIndex(
                name: "IX_SessaoAcesso_Chave",
                table: "SessaoAcesso");

            migrationBuilder.DropColumn(
                name: "OffsetReferenciaVencimento",
                table: "CartaoCredito");

            migrationBuilder.AddForeignKey(
                name: "FK_SessaoAcesso_Usuario_UsuarioId",
                table: "SessaoAcesso",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
