using Milabowl.Processing.DataImport;

namespace Milabowl.Processing.Processing.Rules;

public class RedCards: IMilaRule
{
    public MilaRuleResult Calculate(UserGameWeek userGameWeek)
    {
        return new MilaRuleResult(
            "RedCard",
            userGameWeek.Lineup.Where(pe => pe.RedCards == 1).Sum(pe => pe.Multiplier)
        );
    }
}