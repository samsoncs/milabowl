using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class SixtyNine : MilaRule
{
    protected override string ShortName => "69";
    protected override string Description => "Receive 6.9 pts if a total score of 69.";

    protected override RulePoints CalculatePoints(ManagerGameWeekState userGameWeek)
    {
        var totalTeamScore = userGameWeek.Lineup.Sum(pe => pe.TotalPoints * pe.Multiplier);
        var points = totalTeamScore == 69 ? 6.9m : 0;
        return new RulePoints(points, null);
    }
}
