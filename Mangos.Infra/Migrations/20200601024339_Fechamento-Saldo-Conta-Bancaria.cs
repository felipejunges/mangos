using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class FechamentoSaldoContaBancaria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Fechado",
                table: "SaldoContaBancaria",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataHoraFechamento",
                table: "SaldoContaBancaria",
                nullable: true);

            migrationBuilder.Sql("UPDATE SaldoContaBancaria SET Fechado = 1, DataHoraFechamento = GETDATE() WHERE Data <= '2020-04-01'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataHoraFechamento",
                table: "SaldoContaBancaria");

            migrationBuilder.DropColumn(
                name: "Fechado",
                table: "SaldoContaBancaria");
        }
    }
}
