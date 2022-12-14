using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class AddingTeamDetailsViewAndEarlyMatchFunction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"create function GetEarlierMatch(@teamId int)
	                                RETURNS datetime
	                                BEGIN
		                                DECLARE @result DATETIME
		                                SELECT TOP 1 @RESULT = DATE
		                                FROM MATCHES 
		                                ORDER BY DATE
		                                RETURN @RESULT
	                                END");

            migrationBuilder.Sql(@"CREATE VIEW TeamsCoachesLeagues
	                                    AS
                                    SELECT T.NAME, C.NAME AS COACHNAME, L.NAME AS LEAGUENAME
                                    FROM TEAMS AS T
                                    LEFT OUTER JOIN COACHES AS C ON T.ID = C.TEAMID
                                    INNER JOIN LEAGUES AS L ON L.ID = T.LeagueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION [dbo].[GetEarlierMatch]");
            migrationBuilder.Sql(@"DROP VIEW [dbo].[TeamsCoachesLeagues]");
        }
    }
}
