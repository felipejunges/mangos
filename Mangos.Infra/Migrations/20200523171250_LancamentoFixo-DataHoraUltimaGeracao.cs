using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class LancamentoFixoDataHoraUltimaGeracao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataHoraUltimaGeracao",
                table: "LancamentoFixo",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataHoraUltimaGeracao",
                table: "LancamentoFixo");
        }
    }
}
