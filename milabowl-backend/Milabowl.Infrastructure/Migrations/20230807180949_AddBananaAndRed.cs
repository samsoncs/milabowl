using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Milabowl.Migrations
{
    /// <inheritdoc />
    public partial class AddBananaAndRed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BlueShell",
                table: "MilaGWScores",
                newName: "RedShell");

            migrationBuilder.AddColumn<decimal>(
                name: "Banana",
                table: "MilaGWScores",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Banana",
                table: "MilaGWScores");

            migrationBuilder.RenameColumn(
                name: "RedShell",
                table: "MilaGWScores",
                newName: "BlueShell");
        }
    }
}
