using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class HardestThrust : MilaRule
{
    protected override string ShortName => "HT";

    protected override string Description =>
        "Receive 1.6 points if your defenders scored the most goals. No points if tie.";

    protected override RulePoints CalculatePoints(ManagerGameWeekState userGameWeek)
    {
        var goalsScored = GetGoalsByDefender(userGameWeek.Lineup);
        var maxOpponentsGoalsScored = userGameWeek
            .Opponents.Select(u => GetGoalsByDefender(u.Lineup))
            .Max();

        var points = goalsScored > maxOpponentsGoalsScored ? 1.6m : 0;

        return new RulePoints(
            points,
            $"Defenders scored {goalsScored} goals. Max goals scored by opponents: {maxOpponentsGoalsScored}"
        );
    }

    private int GetGoalsByDefender(IReadOnlyList<PlayerEvent> lineup)
    {
        return lineup
            .Where(l => l is { Multiplier: > 0, PlayerPosition: PlayerPosition.DEF })
            .Sum(pe => pe.GoalsScored);
    }
}
