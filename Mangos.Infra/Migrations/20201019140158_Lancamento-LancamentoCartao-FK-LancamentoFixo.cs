using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class LancamentoLancamentoCartaoFKLancamentoFixo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LancamentoFixoOrigemId",
                table: "LancamentoCartao",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LancamentoFixoOrigemId",
                table: "Lancamento",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoCartao_LancamentoFixoOrigemId",
                table: "LancamentoCartao",
                column: "LancamentoFixoOrigemId");

            migrationBuilder.CreateIndex(
                name: "IX_Lancamento_LancamentoFixoOrigemId",
                table: "Lancamento",
                column: "LancamentoFixoOrigemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamento_LancamentoFixo_LancamentoFixoOrigemId",
                table: "Lancamento",
                column: "LancamentoFixoOrigemId",
                principalTable: "LancamentoFixo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LancamentoCartao_LancamentoFixo_LancamentoFixoOrigemId",
                table: "LancamentoCartao",
                column: "LancamentoFixoOrigemId",
                principalTable: "LancamentoFixo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lancamento_LancamentoFixo_LancamentoFixoOrigemId",
                table: "Lancamento");

            migrationBuilder.DropForeignKey(
                name: "FK_LancamentoCartao_LancamentoFixo_LancamentoFixoOrigemId",
                table: "LancamentoCartao");

            migrationBuilder.DropIndex(
                name: "IX_LancamentoCartao_LancamentoFixoOrigemId",
                table: "LancamentoCartao");

            migrationBuilder.DropIndex(
                name: "IX_Lancamento_LancamentoFixoOrigemId",
                table: "Lancamento");

            migrationBuilder.DropColumn(
                name: "LancamentoFixoOrigemId",
                table: "LancamentoCartao");

            migrationBuilder.DropColumn(
                name: "LancamentoFixoOrigemId",
                table: "Lancamento");
        }
    }
}
