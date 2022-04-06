using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class MangosLoggerLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("TRUNCATE TABLE Log");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "MessageTemplate",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "Properties",
                table: "Log");

            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "Log",
                newName: "DataHora");

            migrationBuilder.AlterColumn<string>(
                name: "Exception",
                table: "Log",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(MAX)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Log",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogLevel",
                table: "Log",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Mensagem",
                table: "Log",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "LogLevel",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "Mensagem",
                table: "Log");

            migrationBuilder.RenameColumn(
                name: "DataHora",
                table: "Log",
                newName: "TimeStamp");

            migrationBuilder.AlterColumn<string>(
                name: "Exception",
                table: "Log",
                type: "nvarchar(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "Log",
                type: "nvarchar(128)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Log",
                type: "nvarchar(MAX)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MessageTemplate",
                table: "Log",
                type: "nvarchar(MAX)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "Log",
                type: "nvarchar(MAX)",
                nullable: true);
        }
    }
}
