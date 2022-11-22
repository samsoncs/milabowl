using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Milabowl.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<Guid>(nullable: false),
                    FantasyEventId = table.Column<int>(nullable: false),
                    Deadline = table.Column<DateTime>(nullable: false),
                    Finished = table.Column<bool>(nullable: false),
                    DataChecked = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    GameWeek = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    LeagueId = table.Column<Guid>(nullable: false),
                    FantasyLeagueId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Closed = table.Column<bool>(nullable: false),
                    LeagueType = table.Column<string>(nullable: true),
                    Scoring = table.Column<string>(nullable: true),
                    AdminEntry = table.Column<int>(nullable: false),
                    StartEvent = table.Column<int>(nullable: false),
                    CodePrivacy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => x.LeagueId);
                });

            migrationBuilder.CreateTable(
                name: "MilaGWScores",
                columns: table => new
                {
                    MilaGWScoreId = table.Column<Guid>(nullable: false),
                    GW = table.Column<string>(nullable: true),
                    TeamName = table.Column<string>(nullable: true),
                    Hit = table.Column<decimal>(nullable: false),
                    CapFail = table.Column<decimal>(nullable: false),
                    BenchFail = table.Column<decimal>(nullable: false),
                    CapKeep = table.Column<decimal>(nullable: false),
                    CapDef = table.Column<decimal>(nullable: false),
                    GWPosition = table.Column<decimal>(nullable: false),
                    GW69 = table.Column<decimal>(nullable: false),
                    RedCard = table.Column<decimal>(nullable: false),
                    YellowCard = table.Column<decimal>(nullable: false),
                    MinusIsPlus = table.Column<decimal>(nullable: false),
                    IncreaseStreak = table.Column<decimal>(nullable: false),
                    EqualStreak = table.Column<decimal>(nullable: false),
                    GWScore = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilaGWScores", x => x.MilaGWScoreId);
                });

            migrationBuilder.CreateTable(
                name: "MilaTotalScores",
                columns: table => new
                {
                    MilaTotalScoreId = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    TeamName = table.Column<string>(nullable: true),
                    H2H = table.Column<decimal>(nullable: false),
                    MaxGWScore = table.Column<decimal>(nullable: false),
                    MinGWScore = table.Column<decimal>(nullable: false),
                    TeamValue = table.Column<decimal>(nullable: false),
                    NoHands = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilaTotalScores", x => x.MilaTotalScoreId);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamId = table.Column<Guid>(nullable: false),
                    FantasyTeamId = table.Column<int>(nullable: false),
                    FantasyTeamCode = table.Column<int>(nullable: false),
                    TeamName = table.Column<string>(nullable: true),
                    TeamShortName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    EntryName = table.Column<string>(nullable: true),
                    FantasyEntryId = table.Column<int>(nullable: false),
                    FantasyResultId = table.Column<int>(nullable: false),
                    Rank = table.Column<int>(nullable: false),
                    LastRank = table.Column<int>(nullable: false),
                    EventTotal = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    PlayerId = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    NowCost = table.Column<int>(nullable: false),
                    FantasyPlayerId = table.Column<int>(nullable: false),
                    FkTeamId = table.Column<Guid>(nullable: false),
                    Code = table.Column<int>(nullable: false),
                    ElementType = table.Column<int>(nullable: false),
                    EventPoints = table.Column<int>(nullable: false),
                    Form = table.Column<string>(nullable: true),
                    InDreamteam = table.Column<bool>(nullable: false),
                    News = table.Column<string>(nullable: true),
                    NewsAdded = table.Column<DateTime>(nullable: true),
                    Photo = table.Column<string>(nullable: true),
                    PointsPerGame = table.Column<string>(nullable: true),
                    SelectedByPercent = table.Column<string>(nullable: true),
                    Special = table.Column<bool>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    TotalPoints = table.Column<int>(nullable: false),
                    TransfersIn = table.Column<int>(nullable: false),
                    TransfersInEvent = table.Column<int>(nullable: false),
                    TransfersOut = table.Column<int>(nullable: false),
                    TransfersOutEvent = table.Column<int>(nullable: false),
                    ValueForm = table.Column<string>(nullable: true),
                    ValueSeason = table.Column<string>(nullable: true),
                    WebName = table.Column<string>(nullable: true),
                    Minutes = table.Column<int>(nullable: false),
                    GoalsScored = table.Column<int>(nullable: false),
                    Assists = table.Column<int>(nullable: false),
                    CleanSheets = table.Column<int>(nullable: false),
                    GoalsConceded = table.Column<int>(nullable: false),
                    OwnGoals = table.Column<int>(nullable: false),
                    PenaltiesSaved = table.Column<int>(nullable: false),
                    PenaltiesMissed = table.Column<int>(nullable: false),
                    YellowCards = table.Column<int>(nullable: false),
                    RedCards = table.Column<int>(nullable: false),
                    Saves = table.Column<int>(nullable: false),
                    Bonus = table.Column<int>(nullable: false),
                    Bps = table.Column<int>(nullable: false),
                    Influence = table.Column<string>(nullable: true),
                    Creativity = table.Column<string>(nullable: true),
                    Threat = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_Players_Teams_FkTeamId",
                        column: x => x.FkTeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lineups",
                columns: table => new
                {
                    LineupId = table.Column<Guid>(nullable: false),
                    FkEventId = table.Column<Guid>(nullable: false),
                    FkUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lineups", x => x.LineupId);
                    table.ForeignKey(
                        name: "FK_Lineups_Events_FkEventId",
                        column: x => x.FkEventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lineups_Users_FkUserId",
                        column: x => x.FkUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLeagues",
                columns: table => new
                {
                    UserLeagueId = table.Column<Guid>(nullable: false),
                    FkUserId = table.Column<Guid>(nullable: false),
                    FkLeagueId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLeagues", x => x.UserLeagueId);
                    table.ForeignKey(
                        name: "FK_UserLeagues_Leagues_FkLeagueId",
                        column: x => x.FkLeagueId,
                        principalTable: "Leagues",
                        principalColumn: "LeagueId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLeagues_Users_FkUserId",
                        column: x => x.FkUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerEvents",
                columns: table => new
                {
                    PlayerEventId = table.Column<Guid>(nullable: false),
                    FantasyPlayerEventId = table.Column<int>(nullable: false),
                    FkPlayerId = table.Column<Guid>(nullable: false),
                    FkEventId = table.Column<Guid>(nullable: false),
                    Minutes = table.Column<int>(nullable: false),
                    GoalsScored = table.Column<int>(nullable: false),
                    Assists = table.Column<int>(nullable: false),
                    CleanSheets = table.Column<int>(nullable: false),
                    GoalsConceded = table.Column<int>(nullable: false),
                    OwnGoals = table.Column<int>(nullable: false),
                    PenaltiesSaved = table.Column<int>(nullable: false),
                    PenaltiesMissed = table.Column<int>(nullable: false),
                    YellowCards = table.Column<int>(nullable: false),
                    RedCards = table.Column<int>(nullable: false),
                    Saves = table.Column<int>(nullable: false),
                    Bonus = table.Column<int>(nullable: false),
                    Bps = table.Column<int>(nullable: false),
                    Influence = table.Column<string>(nullable: true),
                    Creativity = table.Column<string>(nullable: true),
                    Threat = table.Column<string>(nullable: true),
                    IctIndex = table.Column<string>(nullable: true),
                    TotalPoints = table.Column<int>(nullable: false),
                    InDreamteam = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerEvents", x => x.PlayerEventId);
                    table.ForeignKey(
                        name: "FK_PlayerEvents_Events_FkEventId",
                        column: x => x.FkEventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerEvents_Players_FkPlayerId",
                        column: x => x.FkPlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerEventLineups",
                columns: table => new
                {
                    PlayerEventLineupId = table.Column<Guid>(nullable: false),
                    FkPlayerEventId = table.Column<Guid>(nullable: false),
                    FkLineupId = table.Column<Guid>(nullable: false),
                    Multiplier = table.Column<int>(nullable: false),
                    IsCaptain = table.Column<bool>(nullable: false),
                    IsViceCaptain = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerEventLineups", x => x.PlayerEventLineupId);
                    table.ForeignKey(
                        name: "FK_PlayerEventLineups_Lineups_FkLineupId",
                        column: x => x.FkLineupId,
                        principalTable: "Lineups",
                        principalColumn: "LineupId");
                    table.ForeignKey(
                        name: "FK_PlayerEventLineups_PlayerEvents_FkPlayerEventId",
                        column: x => x.FkPlayerEventId,
                        principalTable: "PlayerEvents",
                        principalColumn: "PlayerEventId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lineups_FkEventId",
                table: "Lineups",
                column: "FkEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Lineups_FkUserId",
                table: "Lineups",
                column: "FkUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerEventLineups_FkLineupId",
                table: "PlayerEventLineups",
                column: "FkLineupId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerEventLineups_FkPlayerEventId",
                table: "PlayerEventLineups",
                column: "FkPlayerEventId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerEvents_FkEventId",
                table: "PlayerEvents",
                column: "FkEventId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerEvents_FkPlayerId",
                table: "PlayerEvents",
                column: "FkPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_FkTeamId",
                table: "Players",
                column: "FkTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLeagues_FkLeagueId",
                table: "UserLeagues",
                column: "FkLeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLeagues_FkUserId",
                table: "UserLeagues",
                column: "FkUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MilaGWScores");

            migrationBuilder.DropTable(
                name: "MilaTotalScores");

            migrationBuilder.DropTable(
                name: "PlayerEventLineups");

            migrationBuilder.DropTable(
                name: "UserLeagues");

            migrationBuilder.DropTable(
                name: "Lineups");

            migrationBuilder.DropTable(
                name: "PlayerEvents");

            migrationBuilder.DropTable(
                name: "Leagues");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
