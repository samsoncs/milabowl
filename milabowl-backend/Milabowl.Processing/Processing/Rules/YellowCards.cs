using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class YellowCards : MilaRule
{
    protected override string ShortName => "YC";
    protected override string Description => "Receive 1 point pr. yellow card. Captains double score.";

    protected override RulePoints CalculatePoints(MilaGameWeekState userGameWeek)
    {
        var points = userGameWeek.User.Lineup.Where(pe => pe.YellowCards == 1).Sum(pe => pe.Multiplier);
        return new RulePoints(points, null);
    }
}
