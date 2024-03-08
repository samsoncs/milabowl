using Milabowl.Processing.DataImport;
using Milabowl.Processing.DataImport.Models;

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
