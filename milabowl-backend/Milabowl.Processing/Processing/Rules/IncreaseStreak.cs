using Milabowl.Processing.DataImport;
using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class IncreaseStreak : IMilaRule
{
    public MilaRuleResult Calculate(UserGameWeek userGameWeek)
    {
        var hasStreak = !(userGameWeek.UserHistory.Count < 2);
        var prevGameWeekScore = 0.0m;
        foreach (
            var gameWeek in userGameWeek
                .UserHistory.OrderByDescending(u => u.Event.GameWeek)
                .Take(3)
        )
        {
            if (gameWeek.FplScores.TotalScore < prevGameWeekScore)
            {
                hasStreak = false;
                break;
            }

            prevGameWeekScore = gameWeek.FplScores.TotalScore;
        }

        var points = 0.0m;
        if (hasStreak && userGameWeek.FplScores.TotalScore > prevGameWeekScore)
        {
            points = 1;
        }

        return new MilaRuleResult("IncreaseStreak", "IS", points);
    }
}
