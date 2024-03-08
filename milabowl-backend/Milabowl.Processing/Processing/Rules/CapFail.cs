using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class CapFailScore : MilaRule
{
    public override string ShortName => "CF";

    protected override decimal CalculatePoints(UserGameWeek userGameWeek)
    {
        return userGameWeek.Lineup.Any(pe => pe is { IsCaptain: true, TotalPoints: < 5 }) ? -1 : 0;
    }
}
