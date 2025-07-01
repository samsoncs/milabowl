using Dapper;
using Microsoft.Data.SqlClient;
using Milabowl.Processing.DataImport;

namespace Milabowl.Milalytics;

public static class FplDataImporter
{
    public static async Task ImportToSql(FplImporter importer)
    {
        await CleanDatabase();

        Console.WriteLine("Fetching FplData from APIs");
        var importedData = await importer.ImportFplDataForRulesProcessing();
        Console.WriteLine("Fetching FplData from APIs - Finished");

        Console.WriteLine("Importing FplData to SQL Database");
        foreach (var gameWeekState in importedData.ManagerGameWeekStates)
        {
            var managerGameWeek = new ManagerGameWeek(
                Guid.NewGuid(),
                gameWeekState.User.UserName,
                gameWeekState.User.TeamName,
                gameWeekState.User.Id,
                gameWeekState.Event.GameWeek,
                gameWeekState.ActiveChip,
                gameWeekState.TransfersIn.Select(s => $"{s.FirstName} {s.Surname}").ToList(),
                gameWeekState.TransfersOut.Select(s => $"{s.FirstName} {s.Surname}").ToList()
            );

            await using var connection = new SqlConnection(DbConnection.CONNECTION_STRING);
            await connection.ExecuteAsync(
                """
                    INSERT INTO ManagerGameWeek (
                        ManagerGameWeekId,
                        ManagerName,
                        FantasyTeamName,
                        FantasyEntryId,
                        GameWeek,
                        ActiveChip,
                        SubsIn,
                        SubsOut
                    )
                    VALUES (
                        @ManagerGameWeekId,
                        @ManagerName,
                        @FantasyTeamName,
                        @FantasyEntryId,
                        @GameWeek,
                        @ActiveChip,
                        @SubsIn,
                        @SubsOut
                    )
                """,
                new
                {
                    managerGameWeek.ManagerGameWeekId,
                    managerGameWeek.ManagerName,
                    managerGameWeek.FantasyTeamName,
                    managerGameWeek.FantasyEntryId,
                    managerGameWeek.GameWeek,
                    managerGameWeek.ActiveChip,
                    SubsIn = string.Join(",", managerGameWeek.SubsIn),
                    SubsOut = string.Join(",", managerGameWeek.SubsOut),
                }
            );

            foreach (var playerEvent in gameWeekState.Lineup)
            {
                var managerGameWeekLineup = new ManagerGameWeekLineup(
                    managerGameWeek.ManagerGameWeekId,
                    playerEvent.Multiplier,
                    playerEvent.IsCaptain,
                    playerEvent.IsViceCaptain,
                    playerEvent.FantasyPlayerEventId,
                    playerEvent.FirstName,
                    playerEvent.Surname,
                    playerEvent.WebName,
                    1, //playerEvent.PlayerPosition, TODO: Map enum to stirng in DB
                    playerEvent.FantasyTeamId,
                    playerEvent.FantasyTeamCode,
                    playerEvent.TeamName,
                    playerEvent.TeamShortName,
                    playerEvent.Minutes,
                    playerEvent.GoalsScored,
                    playerEvent.Assists,
                    playerEvent.CleanSheets,
                    playerEvent.GoalsConceded,
                    playerEvent.OwnGoals,
                    playerEvent.PenaltiesSaved,
                    playerEvent.PenaltiesMissed,
                    playerEvent.YellowCards,
                    playerEvent.RedCards,
                    playerEvent.Saves,
                    playerEvent.Bonus,
                    playerEvent.Bps,
                    playerEvent.Influence,
                    playerEvent.Creativity,
                    playerEvent.Threat,
                    playerEvent.IctIndex,
                    playerEvent.TotalPoints,
                    playerEvent.InDreamteam
                );

                await using var cnn = new SqlConnection(DbConnection.CONNECTION_STRING);
                await cnn.ExecuteAsync(
                    """
                        INSERT INTO ManagerGameWeekLineup (
                            FkManagerGameWeekId,
                            Multiplier,
                            IsCaptain,
                            IsViceCaptain,
                            FantasyPlayerEventId,
                            FirstName,
                            LastName,
                            WebName,
                            ElementType,
                            FantasyTeamId,
                            FantasyTeamCode,
                            TeamName,
                            TeamShortName,
                            Minutes,
                            GoalsScored,
                            Assists,
                            CleanSheets,
                            GoalsConceded,
                            OwnGoals,
                            PenaltiesSaved,
                            PenaltiesMissed,
                            YellowCards,
                            RedCards,
                            Saves,
                            Bonus,
                            Bps,
                            Influence,
                            Creativity,
                            Threat,
                            IctIndex,
                            TotalPoints,
                            InDreamTeam
                        )
                        VALUES (
                            @FkManagerGameWeekId,
                            @Multiplier,
                            @IsCaptain,
                            @IsViceCaptain,
                            @FantasyPlayerEventId,
                            @FirstName,
                            @LastName,
                            @WebName,
                            @ElementType,
                            @FantasyTeamId,
                            @FantasyTeamCode,
                            @TeamName,
                            @TeamShortName,
                            @Minutes,
                            @GoalsScored,
                            @Assists,
                            @CleanSheets,
                            @GoalsConceded,
                            @OwnGoals,
                            @PenaltiesSaved,
                            @PenaltiesMissed,
                            @YellowCards,
                            @RedCards,
                            @Saves,
                            @Bonus,
                            @Bps,
                            @Influence,
                            @Creativity,
                            @Threat,
                            @IctIndex,
                            @TotalPoints,
                            @InDreamTeam
                        )
                    """,
                    new
                    {
                        FkManagerGameWeekId = managerGameWeek.ManagerGameWeekId,
                        managerGameWeekLineup.Multiplier,
                        managerGameWeekLineup.IsCaptain,
                        managerGameWeekLineup.IsViceCaptain,
                        managerGameWeekLineup.FantasyPlayerEventId,
                        managerGameWeekLineup.FirstName,
                        managerGameWeekLineup.LastName,
                        managerGameWeekLineup.WebName,
                        managerGameWeekLineup.ElementType,
                        managerGameWeekLineup.FantasyTeamId,
                        managerGameWeekLineup.FantasyTeamCode,
                        managerGameWeekLineup.TeamName,
                        managerGameWeekLineup.TeamShortName,
                        managerGameWeekLineup.Minutes,
                        managerGameWeekLineup.GoalsScored,
                        managerGameWeekLineup.Assists,
                        managerGameWeekLineup.CleanSheets,
                        managerGameWeekLineup.GoalsConceded,
                        managerGameWeekLineup.OwnGoals,
                        managerGameWeekLineup.PenaltiesSaved,
                        managerGameWeekLineup.PenaltiesMissed,
                        managerGameWeekLineup.YellowCards,
                        managerGameWeekLineup.RedCards,
                        managerGameWeekLineup.Saves,
                        managerGameWeekLineup.Bonus,
                        managerGameWeekLineup.Bps,
                        managerGameWeekLineup.Influence,
                        managerGameWeekLineup.Creativity,
                        managerGameWeekLineup.Threat,
                        managerGameWeekLineup.IctIndex,
                        managerGameWeekLineup.TotalPoints,
                        managerGameWeekLineup.InDreamTeam,
                    }
                );
            }
        }

        Console.WriteLine("Importing FplData to SQL Database - finished");
    }

    private static async Task CleanDatabase()
    {
        Console.WriteLine("Cleaning data from database");
        await using var connection = new SqlConnection(DbConnection.CONNECTION_STRING);
        await connection.ExecuteAsync("DELETE FROM ManagerGameWeekLineup");
        await connection.ExecuteAsync("DELETE FROM ManagerGameWeek");
        Console.WriteLine("Cleaning data from database - Finished");
    }
}
