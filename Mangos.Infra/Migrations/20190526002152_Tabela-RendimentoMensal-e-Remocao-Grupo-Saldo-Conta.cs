using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class TabelaRendimentoMensaleRemocaoGrupoSaldoConta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaldoContaBancaria_Grupo_GrupoId",
                table: "SaldoContaBancaria");

            migrationBuilder.DropIndex(
                name: "IX_SaldoContaBancaria_GrupoId",
                table: "SaldoContaBancaria");

            migrationBuilder.DropColumn(
                name: "GrupoId",
                table: "SaldoContaBancaria");

            migrationBuilder.DropColumn(
                name: "DataUltimoFechamento",
                table: "CartaoCredito");

            migrationBuilder.AddColumn<int>(
                name: "CategoriaRendimentoMensalId",
                table: "ContaBancaria",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RendimentosMensalConta",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DataHoraCadastro = table.Column<DateTime>(nullable: false),
                    ContaBancariaId = table.Column<int>(nullable: false),
                    MesReferencia = table.Column<DateTime>(nullable: false),
                    Valor = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RendimentosMensalConta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RendimentosMensalConta_ContaBancaria_ContaBancariaId",
                        column: x => x.ContaBancariaId,
                        principalTable: "ContaBancaria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContaBancaria_CategoriaRendimentoMensalId",
                table: "ContaBancaria",
                column: "CategoriaRendimentoMensalId");

            migrationBuilder.CreateIndex(
                name: "IX_RendimentosMensalConta_ContaBancariaId",
                table: "RendimentosMensalConta",
                column: "ContaBancariaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContaBancaria_Categoria_CategoriaRendimentoMensalId",
                table: "ContaBancaria",
                column: "CategoriaRendimentoMensalId",
                principalTable: "Categoria",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContaBancaria_Categoria_CategoriaRendimentoMensalId",
                table: "ContaBancaria");

            migrationBuilder.DropTable(
                name: "RendimentosMensalConta");

            migrationBuilder.DropIndex(
                name: "IX_ContaBancaria_CategoriaRendimentoMensalId",
                table: "ContaBancaria");

            migrationBuilder.DropColumn(
                name: "CategoriaRendimentoMensalId",
                table: "ContaBancaria");

            migrationBuilder.AddColumn<int>(
                name: "GrupoId",
                table: "SaldoContaBancaria",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataUltimoFechamento",
                table: "CartaoCredito",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SaldoContaBancaria_GrupoId",
                table: "SaldoContaBancaria",
                column: "GrupoId");

            migrationBuilder.AddForeignKey(
                name: "FK_SaldoContaBancaria_Grupo_GrupoId",
                table: "SaldoContaBancaria",
                column: "GrupoId",
                principalTable: "Grupo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
