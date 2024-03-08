using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class GameWeekPosition : MilaRule
{
    public override string ShortName => "GW PS";

    protected override decimal CalculatePoints(UserGameWeek userGameWeek)
    {
        var rulePoints = 0.0m;
        var iteration = 0.0m;
        var isSoleHighestScore = true;
        foreach (
            var grp in userGameWeek
                .Opponents.OrderBy(m => m.FplScores.TotalScore)
                .GroupBy(g => g.FplScores.TotalScore)
        )
        {
            if (userGameWeek.FplScores.TotalScore <= grp.Key)
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

        return rulePoints;
    }
}
