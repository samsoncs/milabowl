using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Milabowl.Migrations
{
    public partial class AddNewValuesToPlayerEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Selected",
                table: "PlayerEvents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransferBalance",
                table: "PlayerEvents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransfersIn",
                table: "PlayerEvents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransfersOut",
                table: "PlayerEvents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                table: "PlayerEvents",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Selected",
                table: "PlayerEvents");

            migrationBuilder.DropColumn(
                name: "TransferBalance",
                table: "PlayerEvents");

            migrationBuilder.DropColumn(
                name: "TransfersIn",
                table: "PlayerEvents");

            migrationBuilder.DropColumn(
                name: "TransfersOut",
                table: "PlayerEvents");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "PlayerEvents");
        }
    }
}
