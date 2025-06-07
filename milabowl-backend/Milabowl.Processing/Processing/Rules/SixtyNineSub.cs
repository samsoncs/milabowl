using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class SixtyNineSub : MilaRule
{
    protected override string ShortName => "69Sub";
    protected override string Description => "Receive 2.69 if you have at least one player that played 69 minutes. Captain doubles socre.";

    protected override RulePoints CalculatePoints(MilaGameWeekState userGameWeek)
    {
        var capMultiplier = userGameWeek.User.Lineup.FirstOrDefault(pe => pe is { IsCaptain: true, Minutes: 69 })?.Multiplier ?? 1;
        var points = userGameWeek.User.Lineup.Any(pe => pe.Minutes == 69) ? 2.69m * capMultiplier : 0;
        return new RulePoints(points, null);
    }
}
