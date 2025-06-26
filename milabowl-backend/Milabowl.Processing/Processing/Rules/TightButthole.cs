using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class TightButthole: MilaRule
{
    protected override string ShortName => "*";
    protected override string Description => "Receive 2.1 points if your starting 11 conceded fewest goals (goals are counted for every player on a team).";

    protected override RulePoints CalculatePoints(MilaGameWeekState userGameWeek)
    {
        var goalsConceded = userGameWeek.Lineup
            .Where(l => l.Multiplier > 0)
            .Sum(l => l.GoalsConceded);

        var minOpponentsGoalsConceded = userGameWeek.Opponents
            .Min(o =>
                o.Lineup.Where(l => l.Multiplier > 0)
                    .Sum(l => l.GoalsConceded)
            );

        var points = goalsConceded < minOpponentsGoalsConceded ? 2.1m : 0;
        return new RulePoints(points, $"{goalsConceded} goals conceded. Min goals conceded by opponent: {minOpponentsGoalsConceded}.");
    }
}
