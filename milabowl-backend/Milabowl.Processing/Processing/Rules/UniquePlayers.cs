using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class UniquePlayers: MilaRule
{
    protected override string ShortName => "UnqP";
    protected override string Description => "Game week scores are weighted based on uniqueness of the player. If a player is owned by 5 or more players their score counts as 0. The 3 top scoring teams receive 3,2 or 1 points according to team score.";

    protected override RulePoints CalculatePoints(ManagerGameWeekState userGameWeek)
    {
        var playerCountsById = userGameWeek
            .Opponents
            .SelectMany(p => p.Lineup)
            .Concat(userGameWeek.Lineup)
            .Where(pe => pe.Multiplier > 0)
            .GroupBy(pe => pe.FantasyPlayerEventId)
            .ToDictionary(k => k.Key, v => v.Count());

        var weightedPoints = userGameWeek.Lineup.Where(l => l.Multiplier > 0).Sum(u => GetWeightedPoints(u, userGameWeek.Opponents.Count + 1, playerCountsById));
        var playersInFront = userGameWeek.Opponents
            .Select(o => o.Lineup.Where(l => l.Multiplier > 0).Sum(pe => GetWeightedPoints(pe, userGameWeek.Opponents.Count + 1, playerCountsById)))
            .Count(s => s > weightedPoints);

        var reasoning = $"Unique weighted score of: {weightedPoints}";
        return playersInFront switch
        {
            0 => new RulePoints(3, reasoning),
            1 => new RulePoints(2, reasoning),
            2 => new RulePoints(1, reasoning),
            _ => new RulePoints(0, reasoning)
        };
    }

    private decimal GetWeightedPoints(PlayerEvent pe, int numberOfLeaguePlayers,
        IDictionary<int, int> playerCountsById)
    {
        if (playerCountsById[pe.FantasyPlayerEventId] > 4)
        {
            return 0;
        }

        var rarenessFactor = 1 + ((decimal)1 / numberOfLeaguePlayers) -
                             ((decimal)playerCountsById[pe.FantasyPlayerEventId] /
                              numberOfLeaguePlayers);

        return pe.TotalPoints * pe.Multiplier * rarenessFactor;
    }
}
