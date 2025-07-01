using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class MinusIsPlus : MilaRule
{
    protected override string ShortName => "MiP";
    protected override string Description =>
        "Receive all negative points in starting 11 as points. Captains count double.";

    protected override RulePoints CalculatePoints(ManagerGameWeekState userGameWeek)
    {
        var points = userGameWeek
            .Lineup.Where(pe => pe.TotalPoints < 0)
            .Sum(pe => pe.TotalPoints * -1 * pe.Multiplier);
        return new RulePoints(points, null);
    }
}
