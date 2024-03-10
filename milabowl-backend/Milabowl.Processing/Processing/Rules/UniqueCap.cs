using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class UniqueCap : MilaRule
{
    protected override string ShortName => "Unq Cap";

    protected override decimal CalculatePoints(MilaGameWeekState userGameWeek)
    {
        var cap = userGameWeek.User.Lineup.First(l => l.IsCaptain);

        return userGameWeek.Opponents.Any(o =>
        {
            var opCap = o.Lineup.First(l => l.IsCaptain);
            return opCap.FantasyPlayerEventId == cap.FantasyPlayerEventId;
        })
            ? 0
            : 2;
    }
}
