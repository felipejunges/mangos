using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class RemocaoDbFunctionsRoundDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION dbo.[RoundMonth]");
            migrationBuilder.Sql(@"DROP FUNCTION dbo.[RoundMonthComdia]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE FUNCTION dbo.[RoundMonth] (@Data AS [DateTime])
RETURNS [DateTime] AS
BEGIN
    RETURN(SELECT CAST(CAST(DATEPART(YEAR, @Data) AS VARCHAR) +'-'+ CAST(DATEPART(MONTH, @Data) AS VARCHAR) +'-01' AS DATETIME))
END");

            migrationBuilder.Sql(@"CREATE FUNCTION dbo.[RoundMonthComdia] (@Data AS [DateTime], @Dia AS [int])
RETURNS [DateTime] AS
BEGIN
    RETURN(SELECT CAST(CAST(DATEPART(YEAR, @Data) AS VARCHAR) +'-'+ CAST(DATEPART(MONTH, @Data) AS VARCHAR) +'-'+ CAST(@Dia AS VARCHAR) AS DATETIME))
END");
        }
    }
}