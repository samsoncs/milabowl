namespace Milabowl.Processing.DataImport.Models;

public record ImportData
{
    public required IReadOnlyList<ManagerGameWeekState> ManagerGameWeekStates { get; init; }
    public bool IsLive { get; init; }
}
