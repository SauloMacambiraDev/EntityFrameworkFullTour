using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class AddedDefaultLeaguesTeamsAndCoaches : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Leagues",
                columns: new[] { "Id", "Name", "UpdatedAt" },
                values: new object[] { 20, "Default League Sample", null });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "LeagueId", "Name", "UpdatedAt" },
                values: new object[] { 20, 20, "Trevoir Williams - Sample Team", null });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "LeagueId", "Name", "UpdatedAt" },
                values: new object[] { 21, 20, "Trevoir Williams - Sample Team", null });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "LeagueId", "Name", "UpdatedAt" },
                values: new object[] { 22, 20, "Trevoir Williams - Sample Team", null });

            migrationBuilder.InsertData(
                table: "Coaches",
                columns: new[] { "Id", "Name", "TeamId", "UpdatedAt" },
                values: new object[] { 20, "Trevoir Williams", 20, null });

            migrationBuilder.InsertData(
                table: "Coaches",
                columns: new[] { "Id", "Name", "TeamId", "UpdatedAt" },
                values: new object[] { 21, "Trevoir Williams - Sample 1", 21, null });

            migrationBuilder.InsertData(
                table: "Coaches",
                columns: new[] { "Id", "Name", "TeamId", "UpdatedAt" },
                values: new object[] { 22, "Trevoir Williams - Sample 2", 22, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Coaches",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Coaches",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Coaches",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Leagues",
                keyColumn: "Id",
                keyValue: 20);
        }
    }
}
