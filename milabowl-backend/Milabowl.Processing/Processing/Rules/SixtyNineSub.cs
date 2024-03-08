using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class SixtyNineSub : IMilaRule
{
    public MilaRuleResult Calculate(UserGameWeek userGameWeek)
    {
        var cap = userGameWeek.Lineup.First(pe => pe.IsCaptain);
        var points = userGameWeek.Lineup.Any(pe => pe.Minutes == 69) ? 2.69m * cap.Multiplier : 0;
        return new MilaRuleResult("SixtyNineSub", "69Sub", points);
    }
}
