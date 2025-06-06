using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class HardestThrust: MilaRule
{
    protected override string ShortName => "HT";
    protected override decimal CalculatePoints(MilaGameWeekState userGameWeek)
    {
        var goalsScored = GetGoalsByDefender(userGameWeek.User.Lineup);
        var maxOpponentsGoalsScored = userGameWeek
            .Opponents.Select(u => GetGoalsByDefender(u.Lineup)).Max();

        return goalsScored > maxOpponentsGoalsScored ? 1.6m : 0;
    }

    private int GetGoalsByDefender(IReadOnlyList<PlayerEvent> lineup)
    {
        return lineup
            .Where(l => l is { Multiplier: > 0, PlayerPosition: PlayerPosition.DEF })
            .Sum(pe => pe.GoalsConceded);
    }
}
