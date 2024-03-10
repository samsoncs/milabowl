using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class CapDef : MilaRule
{
    protected override string ShortName => "CD";

    protected override decimal CalculatePoints(MilaGameWeekState userGameWeek)
    {
        return userGameWeek.User.Lineup.Any(pe =>
            pe.PlayerPosition == 2 && pe is { IsCaptain: true, Minutes: > 45 }
        )
            ? 1
            : 0;
    }
}
