using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class RemocaoLogEventoCriacaoLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogEvento");

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Message = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    MessageTemplate = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Exception = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.CreateTable(
                name: "LogEvento",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DataHora = table.Column<DateTime>(type: "datetime", nullable: false),
                    EventId = table.Column<int>(nullable: false),
                    LogLevel = table.Column<string>(maxLength: 20, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(MAX)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEvento", x => x.Id);
                });
        }
    }
}
