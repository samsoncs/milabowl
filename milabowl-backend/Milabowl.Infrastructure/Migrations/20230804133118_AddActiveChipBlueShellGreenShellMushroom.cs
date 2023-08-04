using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Milabowl.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveChipBlueShellGreenShellMushroom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActiveChip",
                table: "MilaGWScores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BlueShell",
                table: "MilaGWScores",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GreenShell",
                table: "MilaGWScores",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Mushroom",
                table: "MilaGWScores",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveChip",
                table: "MilaGWScores");

            migrationBuilder.DropColumn(
                name: "BlueShell",
                table: "MilaGWScores");

            migrationBuilder.DropColumn(
                name: "GreenShell",
                table: "MilaGWScores");

            migrationBuilder.DropColumn(
                name: "Mushroom",
                table: "MilaGWScores");
        }
    }
}
