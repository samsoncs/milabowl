using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class IncreaseStreak : MilaRule
{
    protected override string ShortName => "IS";

    protected override decimal CalculatePoints(MilaGameWeekState userGameWeek)
    {
        var hasStreak = !(userGameWeek.User.History.Count < 2);
        var prevGameWeekScore = 0.0m;
        foreach (
            var gameWeek in userGameWeek
                .User.History.OrderByDescending(u => u.Event.GameWeek)
                .Take(3)
        )
        {
            if (gameWeek.TotalScore < prevGameWeekScore)
            {
                hasStreak = false;
                break;
            }

            prevGameWeekScore = gameWeek.TotalScore;
        }

        var points = 0.0m;
        if (hasStreak && userGameWeek.User.TotalScore > prevGameWeekScore)
        {
            points = 1;
        }

        return points;
    }
}
