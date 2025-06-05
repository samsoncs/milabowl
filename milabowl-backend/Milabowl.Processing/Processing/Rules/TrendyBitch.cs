using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class TrendyBitch : MilaRule
{
    protected override string ShortName => "Trnd";

    protected override decimal CalculatePoints(MilaGameWeekState milaGameWeekState)
    {
        var points = 0.0m;
        var trendyBitchInPoints = GetTrendyBitchPoints(
            milaGameWeekState.User.SubsIn.ToList(),
            milaGameWeekState.User.SubsIn
                .Concat(milaGameWeekState.Opponents.SelectMany(o => o.SubsIn)).ToList(),
            true
        );

        var trendyBitchOutPoints = GetTrendyBitchPoints(
            milaGameWeekState.User.SubsOut.ToList(),
            milaGameWeekState.User.SubsOut
                .Concat(milaGameWeekState.Opponents.SelectMany(o => o.SubsOut)).ToList(),
            false
        );

        points += trendyBitchInPoints.Points + trendyBitchOutPoints.Points;

        return points;
    }

    private PointsAndReasoning GetTrendyBitchPoints(IList<Sub> userSubs, IList<Sub> allSubs, bool tradeIn)
    {
        var tradeCounts =
            allSubs
                .GroupBy(s => s.FantasyPlayerEventId)
                .Select(g => new { Player = g.Key, Count = g.Count() })
                .OrderByDescending(p => p.Count)
                .ToList();

        var mostTradedInPlayer = tradeCounts.FirstOrDefault();
        if (
            mostTradedInPlayer is not null
            && allSubs.Count > 1
            && userSubs.Any(a => a.FantasyPlayerEventId == mostTradedInPlayer.Player)
            && tradeCounts.Count(p => p.Count == mostTradedInPlayer.Count) == 1
        )
        {
            // Player did the most popular trade
            var tradedInString = tradeIn ? "in" : "out";
            var player =
                allSubs.First(a => a.FantasyPlayerEventId == mostTradedInPlayer.Player);
            return new PointsAndReasoning(-1, $"Traded {tradedInString} most popular trade: {player.FirstName} {player.Surname}");
        }

        return new PointsAndReasoning(0, null);
    }

    private record PointsAndReasoning(int Points, string? Reasoning);
}
