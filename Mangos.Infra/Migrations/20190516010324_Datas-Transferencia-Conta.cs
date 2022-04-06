using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class DatasTransferenciaConta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataCredito",
                table: "TransferenciaConta",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataDebito",
                table: "TransferenciaConta",
                type: "datetime",
                nullable: true);

            migrationBuilder.Sql("UPDATE TransferenciaConta SET DataDebito = [Data] WHERE ContaBancariaOrigemId IS NOT NULL");
            migrationBuilder.Sql("UPDATE TransferenciaConta SET DataCredito = [Data] WHERE ContaBancariaDestinoId IS NOT NULL");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "TransferenciaConta");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataCredito",
                table: "TransferenciaConta");

            migrationBuilder.DropColumn(
                name: "DataDebito",
                table: "TransferenciaConta");

            migrationBuilder.AddColumn<DateTime>(
                name: "Data",
                table: "TransferenciaConta",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
