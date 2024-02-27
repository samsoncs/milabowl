using Milabowl.Processing.DataImport;

namespace Milabowl.Processing.Processing.Rules;

public class UniqueCap: IMilaRule
{
    public MilaRuleResult Calculate(UserGameWeek userGameWeek)
    {
        var cap = userGameWeek.Lineup.First(l => l.IsCaptain);

        return new MilaRuleResult(
            "Unique Cap",
            "Unq Cap",
                userGameWeek.Opponents.Any(o =>
                {
                    var opCap = o.Lineup.First(l => l.IsCaptain);
                    return opCap.FantasyPlayerEventId == cap.FantasyPlayerEventId;
                }) ? 0 : 2
            );
    }
}
