using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Milabowl.Migrations
{
    public partial class RemoveHit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hit",
                table: "MilaGWScores");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Hit",
                table: "MilaGWScores",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
