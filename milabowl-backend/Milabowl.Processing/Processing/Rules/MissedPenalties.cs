using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class MissedPenalties : MilaRule
{
    protected override string ShortName => "MP";
    protected override string Description => "Receive 1.69 pts for all missed penalties. Captains double score.";

    protected override RulePoints CalculatePoints(MilaGameWeekState userGameWeek)
    {
        var points = userGameWeek.User.Lineup.Sum(p =>
            (p.PenaltiesMissed > 0 ? 1.69m : 0) * p.Multiplier
        );

        return new RulePoints(points, null);
    }
}
