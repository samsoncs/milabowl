using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Milabowl.Migrations
{
    public partial class AddingNewRules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HeadToHeadMeta",
                table: "MilaGWScores",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SixtyNineSub",
                table: "MilaGWScores",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UniqueCap",
                table: "MilaGWScores",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeadToHeadMeta",
                table: "MilaGWScores");

            migrationBuilder.DropColumn(
                name: "SixtyNineSub",
                table: "MilaGWScores");

            migrationBuilder.DropColumn(
                name: "UniqueCap",
                table: "MilaGWScores");
        }
    }
}
