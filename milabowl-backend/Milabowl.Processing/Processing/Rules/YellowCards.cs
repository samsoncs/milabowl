using Milabowl.Processing.DataImport;

namespace Milabowl.Processing.Processing.Rules;

public class YellowCards : IMilaRule
{
    public MilaRuleResult Calculate(UserGameWeek userGameWeek)
    {
        return new MilaRuleResult(
            "YellowCards",
            "YC",
            userGameWeek.Lineup.Where(pe => pe.YellowCards == 1).Sum(pe => pe.Multiplier)
        );
    }
}
