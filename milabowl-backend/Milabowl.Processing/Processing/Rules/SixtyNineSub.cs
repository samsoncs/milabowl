using Milabowl.Processing.DataImport;

namespace Milabowl.Processing.Processing.Rules;

public class SixtyNineSub: IMilaRule
{
    public MilaRuleResult Calculate(UserGameWeek userGameWeek)
    {
        decimal points = 0;
        var cap = userGameWeek.Lineup.FirstOrDefault(pe => pe.IsCaptain);
        if (cap is { Minutes: 69 })
        {
            points = 2.69m * cap.Multiplier;
        }

        points = userGameWeek.Lineup.Any(pe => pe.Minutes == 69) ? 2.69m : 0;
        return new MilaRuleResult(
            "SixtyNineSub",
            "69Sub",
            points
        );
    }
}
