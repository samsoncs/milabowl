using Milabowl.Processing.DataImport;

namespace Milabowl.Processing.Processing.Rules;

public class EqualStreak : IMilaRule
{
    public MilaRuleResult Calculate(UserGameWeek userGameWeek)
    {
        var points = 0.0m;

        var previousGameWeek = userGameWeek.PreviousGameWeek;
        if (previousGameWeek is not null)
        {
            points = userGameWeek.TotalScore == previousGameWeek.TotalScore ? 6.9m : 0;
        }

        return new MilaRuleResult("EqualStreak", "ES", points);
    }
}
