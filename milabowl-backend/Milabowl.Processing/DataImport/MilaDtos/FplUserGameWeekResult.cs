namespace Milabowl.Processing.DataImport.MilaDtos;

public record FplUserGameWeekResult(
    int GameWeek,
    string TeamName,
    int TotalScore,
    IList<FplPlayerEventResult> Lineup
);
