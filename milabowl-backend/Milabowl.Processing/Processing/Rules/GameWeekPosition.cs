using Milabowl.Processing.DataImport;

namespace Milabowl.Processing.Processing.Rules;

public class GameWeekPosition : IMilaRule
{
    public MilaRuleResult Calculate(UserGameWeek userGameWeek)
    {
        var rulePoints = 0.0m;
        var iteration = 0.0m;
        var isSoleHighestScore = true;
        foreach (
            var grp in userGameWeek.Opponents.OrderBy(m => m.TotalScore).GroupBy(g => g.TotalScore)
        )
        {
            if (userGameWeek.TotalScore <= grp.Key)
            {
                rulePoints = iteration / 2.0m;
                isSoleHighestScore = false;
                break;
            }

            iteration++;
        }

        if (isSoleHighestScore)
        {
            rulePoints = iteration / 2;
        }

        return new MilaRuleResult("GameWeekPositionScore", "GW PS", rulePoints);
    }
}
