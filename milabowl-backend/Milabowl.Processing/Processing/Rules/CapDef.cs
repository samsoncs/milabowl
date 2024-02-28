using Milabowl.Processing.DataImport;

namespace Milabowl.Processing.Processing.Rules;

public class CapDef : IMilaRule
{
    public MilaRuleResult Calculate(UserGameWeek userGameWeek)
    {
        return new MilaRuleResult(
            "CapDef",
            "CD",
            userGameWeek.Lineup.Any(pe =>
                pe.PlayerPosition == 2 && pe is { IsCaptain: true, Minutes: > 45 }
            )
                ? 1
                : 0
        );
    }
}
