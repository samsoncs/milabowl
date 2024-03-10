using Milabowl.Processing.DataImport;
using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class EqualStreak : MilaRule
{
    protected override string ShortName => "ES";

    protected override decimal CalculatePoints(MilaGameWeekState userGameWeek)
    {
        var points = 0.0m;

        var previousGameWeek = userGameWeek.User.PreviousGameWeek;
        if (previousGameWeek is not null)
        {
            points = userGameWeek.User.TotalScore == previousGameWeek.TotalScore ? 6.9m : 0;
        }

        return points;
    }
}
