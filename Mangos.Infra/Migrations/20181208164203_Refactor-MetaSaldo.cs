using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class RefactorMetaSaldo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MetaSaldo_ContaBancaria_ContaBancariaId",
                table: "MetaSaldo");

            migrationBuilder.DropIndex(
                name: "IX_MetaSaldo_ContaBancariaId",
                table: "MetaSaldo");

            migrationBuilder.DropColumn(
                name: "ContaBancariaId",
                table: "MetaSaldo");

            migrationBuilder.DropColumn(
                name: "MesFinal",
                table: "MetaSaldo");

            migrationBuilder.RenameColumn(
                name: "ValorFinal",
                table: "MetaSaldo",
                newName: "Valor");

            migrationBuilder.RenameColumn(
                name: "MesInicial",
                table: "MetaSaldo",
                newName: "Mes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Valor",
                table: "MetaSaldo",
                newName: "ValorFinal");

            migrationBuilder.RenameColumn(
                name: "Mes",
                table: "MetaSaldo",
                newName: "MesInicial");

            migrationBuilder.AddColumn<int>(
                name: "ContaBancariaId",
                table: "MetaSaldo",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MesFinal",
                table: "MetaSaldo",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_MetaSaldo_ContaBancariaId",
                table: "MetaSaldo",
                column: "ContaBancariaId");

            migrationBuilder.AddForeignKey(
                name: "FK_MetaSaldo_ContaBancaria_ContaBancariaId",
                table: "MetaSaldo",
                column: "ContaBancariaId",
                principalTable: "ContaBancaria",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
