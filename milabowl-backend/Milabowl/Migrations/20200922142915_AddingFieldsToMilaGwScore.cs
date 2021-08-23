using Microsoft.EntityFrameworkCore.Migrations;

namespace Milabowl.Migrations
{
    public partial class AddingFieldsToMilaGwScore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameWeek",
                table: "MilaGWScores",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalScore",
                table: "MilaGWScores",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "MilaGWScores",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameWeek",
                table: "MilaGWScores");

            migrationBuilder.DropColumn(
                name: "TotalScore",
                table: "MilaGWScores");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "MilaGWScores");
        }
    }
}
