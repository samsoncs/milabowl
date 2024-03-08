using Milabowl.Processing.DataImport;
using Milabowl.Processing.DataImport.Models;

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
