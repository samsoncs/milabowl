using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class RedCards : MilaRule
{
    protected override string ShortName => "RC";
    protected override string Description => "Receive 2 points for all red cards. Captains double score.";

    protected override RulePoints CalculatePoints(MilaGameWeekState userGameWeek)
    {
        var points = userGameWeek.Lineup.Where(pe => pe.RedCards == 1).Sum(pe => pe.Multiplier * 6);
        return new RulePoints(points, null);
    }
}
