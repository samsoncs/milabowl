using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class UniquePlayers: MilaRule
{
    private const decimal TotalPot = 8.125m;
    protected override string ShortName => "UnqP";
    protected override string Description => "Gameweek scores are weighted on uniqueness, and a total pot of 8.125 pts is split according to the percentage of your weighted score to the total weighted score.";

    protected override RulePoints CalculatePoints(MilaGameWeekState userGameWeek)
    {
        var playerCountsById = userGameWeek
            .Opponents
            .SelectMany(p => p.Lineup)
            .Concat(userGameWeek.User.Lineup)
            .Where(pe => pe.Multiplier > 0)
            .GroupBy(pe => pe.FantasyPlayerEventId)
            .ToDictionary(k => k.Key, v => v.Count());

        var weightedPoints = userGameWeek.User.Lineup.Where(l => l.Multiplier > 0).Sum(u => GetWeightedPoints(u, userGameWeek.Opponents.Count + 1, playerCountsById));
        var opponentsWeightedPoints = userGameWeek.Opponents.SelectMany(o => o.Lineup.Where(l => l.Multiplier > 0)).Sum(u => GetWeightedPoints(u, userGameWeek.Opponents.Count + 1, playerCountsById));
        var points = Math.Round(TotalPot * (weightedPoints/opponentsWeightedPoints), 2);

        return new RulePoints(points, null);
    }

    private decimal GetWeightedPoints(PlayerEvent pe, int numberOfLeaguePlayers,
        IDictionary<int, int> playerCountsById)
    {
        var rarenessFactor = 1 + ((decimal)1 / numberOfLeaguePlayers) -
                             ((decimal)playerCountsById[pe.FantasyPlayerEventId] /
                              numberOfLeaguePlayers);

        return pe.TotalPoints * pe.Multiplier * rarenessFactor;
    }
}
