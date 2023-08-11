using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Milabowl.Migrations
{
    /// <inheritdoc />
    public partial class AddDarthMaul : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DarthMaulPoints",
                table: "MilaGWScores",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsDarthMaul",
                table: "MilaGWScores",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDarthMaulContender",
                table: "MilaGWScores",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DarthMaulPoints",
                table: "MilaGWScores");

            migrationBuilder.DropColumn(
                name: "IsDarthMaul",
                table: "MilaGWScores");

            migrationBuilder.DropColumn(
                name: "IsDarthMaulContender",
                table: "MilaGWScores");
        }
    }
}
