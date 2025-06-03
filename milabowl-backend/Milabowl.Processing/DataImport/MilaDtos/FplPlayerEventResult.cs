namespace Milabowl.Processing.DataImport.MilaDtos;

public record FplPlayerEventResult(
    string WebName,
    string TeamName,
    int Points,
    string Position,
    bool IsCap,
    bool IsViceCap,
    bool IsBench
);
