namespace Milabowl.Processing.DataImport.Models;

public record HeadToHeadEvent(
    int Points,
    bool DidWin,
    bool DidDraw,
    bool DidLose,
    int Total,
    bool IsKnockout,
    int LeagueId,
    bool IsBye
);
