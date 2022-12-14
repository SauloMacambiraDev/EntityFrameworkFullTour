using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class AddingStoredProcedureExample : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_GetTeamCoach
                    @teamId int
                AS
                BEGIN
                    SELECT * FROM Coaches where teamId = @teamId
                END
                GO
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE [dbo].[sp_GetTeamCoach]");
        }
    }
}
