using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class HardestThrust: MilaRule
{
    protected override string ShortName => "HT";
    protected override decimal CalculatePoints(MilaGameWeekState userGameWeek)
    {
        var goalsScored = userGameWeek.User.Lineup
            .Where(l => l is { Multiplier: > 0, PlayerPosition: PlayerPosition.DEF })
            .Sum(pe => pe.GoalsConceded);

        var maxOpponentsGoalsScored = userGameWeek
            .Opponents.Select(u => u.Lineup
                .Where(pe => pe is { Multiplier: > 0, PlayerPosition: PlayerPosition.DEF })
                .Sum(pe => pe.GoalsScored)
            ).Max();

        return goalsScored > maxOpponentsGoalsScored ? 1.6m : 0;
    }
}
