using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Milabowl.Migrations
{
    public partial class AddUserHeadToHeadEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MostCaptainedPlayerID",
                table: "Events",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MostSelectedPlayerID",
                table: "Events",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MostTransferredInPlayerID",
                table: "Events",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MostViceCaptainedPlayerID",
                table: "Events",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserHeadToHeadEvents",
                columns: table => new
                {
                    UserHeadToHeadEventID = table.Column<Guid>(nullable: false),
                    FantasyUserHeadToHeadEventID = table.Column<int>(nullable: false),
                    FkUserId = table.Column<Guid>(nullable: false),
                    Points = table.Column<int>(nullable: false),
                    Win = table.Column<int>(nullable: false),
                    Draw = table.Column<int>(nullable: false),
                    Loss = table.Column<int>(nullable: false),
                    Total = table.Column<int>(nullable: false),
                    FkEventId = table.Column<Guid>(nullable: false),
                    IsKnockout = table.Column<bool>(nullable: false),
                    LeagueID = table.Column<int>(nullable: false),
                    IsBye = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHeadToHeadEvents", x => x.UserHeadToHeadEventID);
                    table.ForeignKey(
                        name: "FK_UserHeadToHeadEvents_Events_FkEventId",
                        column: x => x.FkEventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserHeadToHeadEvents_Users_FkUserId",
                        column: x => x.FkUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserHeadToHeadEvents_FkEventId",
                table: "UserHeadToHeadEvents",
                column: "FkEventId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHeadToHeadEvents_FkUserId",
                table: "UserHeadToHeadEvents",
                column: "FkUserId");
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserHeadToHeadEvents");

            migrationBuilder.DropColumn(
                name: "MostCaptainedPlayerID",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MostSelectedPlayerID",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MostTransferredInPlayerID",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MostViceCaptainedPlayerID",
                table: "Events");
        }
    }
}
