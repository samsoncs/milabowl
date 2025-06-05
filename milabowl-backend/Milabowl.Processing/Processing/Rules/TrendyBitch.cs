using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class TrendyBitch : MilaRule
{
    protected override string ShortName => "Trnd";

    protected override decimal CalculatePoints(MilaGameWeekState milaGameWeekState)
    {
        var points = 0.0m;

        var didNotTrade = milaGameWeekState.User.SubsIn.Count == 0 &&
                          milaGameWeekState.User.SubsOut.Count == 0;
        var numberOfNoTrades =
            milaGameWeekState.Opponents.Sum(o =>
                o.SubsIn.Count == 0 && o.SubsOut.Count == 0 ? 1 : 0)
            + (didNotTrade ? 1 : 0);

        var trendyBitchInPoints = GetTrendyBitchPoints(
            milaGameWeekState.User.SubsIn.ToList(),
            milaGameWeekState.User.SubsIn
                .Concat(milaGameWeekState.Opponents.SelectMany(o => o.SubsIn)).ToList(),
            didNotTrade,
            numberOfNoTrades,
            true
        );

        var trendyBitchOutPoints = GetTrendyBitchPoints(
            milaGameWeekState.User.SubsOut.ToList(),
            milaGameWeekState.User.SubsOut
                .Concat(milaGameWeekState.Opponents.SelectMany(o => o.SubsOut)).ToList(),
            didNotTrade,
            numberOfNoTrades,
            false
        );

        points += trendyBitchInPoints.Points + trendyBitchOutPoints.Points;

        return points;
    }

    private PointsAndReasoning GetTrendyBitchPoints(IList<Sub> userSubs, IList<Sub> allSubs, bool didNotTrade,
        int numberOfNoTrades, bool tradeIn)
    {
        var tradeCounts =
            allSubs
                .GroupBy(s => s.FantasyPlayerEventId)
                .Select(g => new { Player = g.Key, Count = g.Count() })
                .OrderByDescending(p => p.Count)
                .ToList();

        var mostTradedInPlayer = tradeCounts.FirstOrDefault();

        if (
            tradeIn &&
            mostTradedInPlayer is not null // At least one trade was made
            && didNotTrade
            && numberOfNoTrades > mostTradedInPlayer.Count
        )
        {
            // Player did the most popular move and did not trade
            return new PointsAndReasoning(1, $"Did not make trade this game week, like {numberOfNoTrades} other players this round.");
        }

        if (
            mostTradedInPlayer is not null
            && userSubs.Any(a => a.FantasyPlayerEventId == mostTradedInPlayer.Player)
            && tradeCounts.Count(p => p.Count == mostTradedInPlayer.Count) == 1
            && mostTradedInPlayer.Count > numberOfNoTrades
        )
        {
            // Player did the most popular trade
            var tradedInString = tradeIn ? "in" : "out";
            var player =
                allSubs.First(a => a.FantasyPlayerEventId == mostTradedInPlayer.Player);
            return new PointsAndReasoning(1, $"Traded {tradedInString} most popular trade: {player.FirstName} {player.Surname}");
        }

        return new PointsAndReasoning(0, null);
    }

    private record PointsAndReasoning(int Points, string? Reasoning);
}
