using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class UniqueCap : MilaRule
{
    protected override string ShortName => "Unq Cap";
    protected override string Description => "Receive 2 points if you have a unique captain.";

    protected override RulePoints CalculatePoints(MilaGameWeekState userGameWeek)
    {
        var cap = userGameWeek.User.Lineup.First(l => l.IsCaptain);

        var points = cap.Minutes < 46 || userGameWeek.Opponents.Any(o =>
        {
            var opCap = o.Lineup.First(l => l.IsCaptain);
            return opCap.FantasyPlayerEventId == cap.FantasyPlayerEventId;
        })
            ? 0
            : 2;
        return new RulePoints(points, null);
    }
}
