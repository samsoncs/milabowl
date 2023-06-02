using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Milabowl.Migrations
{
    /// <inheritdoc />
    public partial class AddUserHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserHistory",
                columns: table => new
                {
                    UserHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FkUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeasonName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPoints = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHistory", x => x.UserHistoryId);
                    table.ForeignKey(
                        name: "FK_UserHistory_Users_FkUserId",
                        column: x => x.FkUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserHistory_FkUserId",
                table: "UserHistory",
                column: "FkUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserHistory");
        }
    }
}
