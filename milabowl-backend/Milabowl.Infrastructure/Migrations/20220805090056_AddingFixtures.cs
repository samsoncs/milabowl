using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Milabowl.Migrations
{
    public partial class AddingFixtures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fixtures",
                columns: table => new
                {
                    FixtureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Finished = table.Column<bool>(type: "bit", nullable: false),
                    FinishedProvisional = table.Column<bool>(type: "bit", nullable: false),
                    FantasyFixtureId = table.Column<int>(type: "int", nullable: false),
                    KickoffTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Minutes = table.Column<int>(type: "int", nullable: false),
                    ProvisionalStartTime = table.Column<bool>(type: "bit", nullable: false),
                    Started = table.Column<bool>(type: "bit", nullable: true),
                    TeamAwayScore = table.Column<int>(type: "int", nullable: true),
                    TeamHomeScore = table.Column<int>(type: "int", nullable: true),
                    TeamHomeDifficulty = table.Column<int>(type: "int", nullable: false),
                    TeamAwayDifficulty = table.Column<int>(type: "int", nullable: false),
                    FkTeamAwayId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FkTeamHomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FkEventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fixtures", x => x.FixtureId);
                    table.ForeignKey(
                        name: "FK_Fixtures_Events_FkEventId",
                        column: x => x.FkEventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fixtures_Teams_FkTeamAwayId",
                        column: x => x.FkTeamAwayId,
                        principalTable: "Teams",
                        principalColumn: "TeamId");
                    table.ForeignKey(
                        name: "FK_Fixtures_Teams_FkTeamHomeId",
                        column: x => x.FkTeamHomeId,
                        principalTable: "Teams",
                        principalColumn: "TeamId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_FkEventId",
                table: "Fixtures",
                column: "FkEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_FkTeamAwayId",
                table: "Fixtures",
                column: "FkTeamAwayId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_FkTeamHomeId",
                table: "Fixtures",
                column: "FkTeamHomeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fixtures");
        }
    }
}
