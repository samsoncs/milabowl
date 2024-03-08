using Milabowl.Processing.DataImport;
using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class UniqueCap : IMilaRule
{
    public MilaRuleResult Calculate(UserGameWeek userGameWeek)
    {
        var cap = userGameWeek.Lineup.First(l => l.IsCaptain);

        return new MilaRuleResult(
            "UniqueCap",
            "Unq Cap",
            userGameWeek.Opponents.Any(o =>
            {
                var opCap = o.Lineup.First(l => l.IsCaptain);
                return opCap.FantasyPlayerEventId == cap.FantasyPlayerEventId;
            })
                ? 0
                : 2
        );
    }
}
