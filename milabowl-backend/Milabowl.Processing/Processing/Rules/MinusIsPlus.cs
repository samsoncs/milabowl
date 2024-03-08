using Milabowl.Processing.DataImport;
using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class MinusIsPlus : IMilaRule
{
    public MilaRuleResult Calculate(UserGameWeek userGameWeek)
    {
        return new MilaRuleResult(
            "MinusIsPlus",
            "MiP",
            userGameWeek
                .Lineup.Where(pe => pe.TotalPoints < 0)
                .Sum(pe => pe.TotalPoints * -1 * pe.Multiplier)
        );
    }
}
