using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class AddDeleteTeamIdSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_DeleteTeamById
                    @teamId int
                AS 
                BEGIN
                    DELETE FROM Teams where Id = @teamId
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE [dbo].[sp_DeleteTeamById]");
        }
    }
}
