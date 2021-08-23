using Microsoft.EntityFrameworkCore.Migrations;

namespace Milabowl.Migrations
{
    public partial class MilaPoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalScore",
                table: "MilaGWScores");

            migrationBuilder.AddColumn<decimal>(
                name: "GWPositionScore",
                table: "MilaGWScores",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MilaPoints",
                table: "MilaGWScores",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GWPositionScore",
                table: "MilaGWScores");

            migrationBuilder.DropColumn(
                name: "MilaPoints",
                table: "MilaGWScores");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalScore",
                table: "MilaGWScores",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
