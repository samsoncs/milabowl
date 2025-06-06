using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class TightButthole: MilaRule
{
    protected override string ShortName => "*";
    protected override decimal CalculatePoints(MilaGameWeekState userGameWeek)
    {
        var goalsConceded = userGameWeek.User.Lineup
            .Where(l => l.Multiplier > 0)
            .Sum(l => l.GoalsConceded);

        var maxOpponentsGoalsConceded = userGameWeek.Opponents
            .Max(o =>
                o.Lineup.Where(l => l.Multiplier > 0)
                    .Sum(l => l.GoalsConceded)
            );

        return goalsConceded > maxOpponentsGoalsConceded ? 2.1m : 0;
    }
}
