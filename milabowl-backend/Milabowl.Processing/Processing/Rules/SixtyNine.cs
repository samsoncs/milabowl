using Milabowl.Processing.DataImport;
using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class SixtyNine : IMilaRule
{
    public MilaRuleResult Calculate(UserGameWeek userGameWeek)
    {
        decimal totalTeamScore = userGameWeek.Lineup.Sum(pe => pe.TotalPoints * pe.Multiplier);
        return new MilaRuleResult("SixtyNine", "69", totalTeamScore == 69 ? 6.9m : 0);
    }
}
