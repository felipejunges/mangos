using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Mangos.Infra.Migrations
{
    public partial class DataHoraExpiracaoSessaoAcessoUsuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataHoraExpiracao",
                table: "SessaoAcesso");

            migrationBuilder.Sql("UPDATE SessaoAcesso SET DataHoraExpiracao = GETDATE()");

            migrationBuilder.AlterColumn<DateTime>("DataHoraExpiracao", "SessaoAcesso", nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataHoraExpiracao",
                table: "SessaoAcesso");
        }
    }
}