using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class SessaoAcessoBrowser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Browser",
                table: "SessaoAcesso",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Browser",
                table: "SessaoAcesso");
        }
    }
}
