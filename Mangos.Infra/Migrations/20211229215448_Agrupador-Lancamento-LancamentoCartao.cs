using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class AgrupadorLancamentoLancamentoCartao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Agrupador",
                table: "LancamentoCartao",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Agrupador",
                table: "Lancamento",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Agrupador",
                table: "LancamentoCartao");

            migrationBuilder.DropColumn(
                name: "Agrupador",
                table: "Lancamento");
        }
    }
}
