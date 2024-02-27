using Milabowl.Processing.DataImport;

namespace Milabowl.Processing.Processing.Rules;

public class MinusIsPlus: IMilaRule
{
    public MilaRuleResult Calculate(UserGameWeek userGameWeek)
    {
        return new MilaRuleResult(
            "MinusIsPlus",
            "MiP",
            userGameWeek.Lineup
                .Where(pe => pe.TotalPoints < 0)
                .Sum(pe => pe.TotalPoints * -1 * pe.Multiplier)
        );
    }
}
