using Milabowl.Processing.DataImport;

namespace Milabowl.Processing.Processing.Rules;

public class MissedPenalties: IMilaRule
{
    public MilaRuleResult Calculate(UserGameWeek userGameWeek)
    {
        return new MilaRuleResult(
            "MissedPenalties",
            "Mp",
            userGameWeek.Lineup.Sum(p => (p.PenaltiesMissed > 0 ? 1.69m : 0) * p.Multiplier)
        );
    }
}
