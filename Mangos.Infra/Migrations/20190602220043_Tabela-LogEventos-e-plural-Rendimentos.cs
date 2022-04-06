using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class TabelaLogEventosepluralRendimentos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RendimentosMensalConta_ContaBancaria_ContaBancariaId",
                table: "RendimentosMensalConta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RendimentosMensalConta",
                table: "RendimentosMensalConta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LogEventos",
                table: "LogEventos");

            migrationBuilder.RenameTable(
                name: "RendimentosMensalConta",
                newName: "RendimentoMensalConta");

            migrationBuilder.RenameTable(
                name: "LogEventos",
                newName: "LogEvento");

            migrationBuilder.RenameIndex(
                name: "IX_RendimentosMensalConta_ContaBancariaId",
                table: "RendimentoMensalConta",
                newName: "IX_RendimentoMensalConta_ContaBancariaId");

            migrationBuilder.RenameColumn(
                name: "Mensagem",
                table: "LogEvento",
                newName: "Message");

            migrationBuilder.AlterColumn<string>(
                name: "LogLevel",
                table: "LogEvento",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(int),
                oldMaxLength: 20);

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "LogEvento",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RendimentoMensalConta",
                table: "RendimentoMensalConta",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LogEvento",
                table: "LogEvento",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RendimentoMensalConta_ContaBancaria_ContaBancariaId",
                table: "RendimentoMensalConta",
                column: "ContaBancariaId",
                principalTable: "ContaBancaria",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RendimentoMensalConta_ContaBancaria_ContaBancariaId",
                table: "RendimentoMensalConta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RendimentoMensalConta",
                table: "RendimentoMensalConta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LogEvento",
                table: "LogEvento");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "LogEvento");

            migrationBuilder.RenameTable(
                name: "RendimentoMensalConta",
                newName: "RendimentosMensalConta");

            migrationBuilder.RenameTable(
                name: "LogEvento",
                newName: "LogEventos");

            migrationBuilder.RenameIndex(
                name: "IX_RendimentoMensalConta_ContaBancariaId",
                table: "RendimentosMensalConta",
                newName: "IX_RendimentosMensalConta_ContaBancariaId");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "LogEventos",
                newName: "Mensagem");

            migrationBuilder.AlterColumn<int>(
                name: "LogLevel",
                table: "LogEventos",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RendimentosMensalConta",
                table: "RendimentosMensalConta",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LogEventos",
                table: "LogEventos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RendimentosMensalConta_ContaBancaria_ContaBancariaId",
                table: "RendimentosMensalConta",
                column: "ContaBancariaId",
                principalTable: "ContaBancaria",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
