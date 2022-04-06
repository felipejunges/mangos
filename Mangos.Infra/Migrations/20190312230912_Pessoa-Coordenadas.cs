using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class PessoaCoordenadas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PessoaCoordenada",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PessoaId = table.Column<int>(nullable: false),
                    Ordem = table.Column<int>(nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(11,8)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(11,8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PessoaCoordenada", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PessoaCoordenada_Pessoa_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PessoaCoordenada_PessoaId",
                table: "PessoaCoordenada",
                column: "PessoaId");

            migrationBuilder.Sql("INSERT INTO PessoaCoordenada(PessoaId, Ordem, Latitude, Longitude) (SELECT Id, 1, Latitude, Longitude FROM Pessoa WHERE Latitude IS NOT NULL AND Longitude IS NOT NULL)");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Pessoa");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Pessoa");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PessoaCoordenada");

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "Pessoa",
                type: "decimal(11,8)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "Pessoa",
                type: "decimal(11,8)",
                nullable: true);
        }
    }
}
