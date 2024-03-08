using Milabowl.Processing.DataImport;
using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class RedCards : IMilaRule
{
    public MilaRuleResult Calculate(UserGameWeek userGameWeek)
    {
        return new MilaRuleResult(
            "RedCard",
            "RC",
            userGameWeek.Lineup.Where(pe => pe.RedCards == 1).Sum(pe => pe.Multiplier)
        );
    }
}
