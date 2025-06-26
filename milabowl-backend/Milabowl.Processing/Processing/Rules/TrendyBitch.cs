using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class TrendyBitch : MilaRule
{
    protected override string ShortName => "Trnd";
    protected override string Description => "Receive -1 penalty point if you sub out the most popular transferred out player, and -1 point if you sub in the most popular transferred in player.";

    protected override RulePoints CalculatePoints(ManagerGameWeekState managerGameWeekState)
    {
        var points = 0.0m;
        var trendyBitchInPoints = GetTrendyBitchPoints(
            managerGameWeekState.SubsIn.ToList(),
            managerGameWeekState.SubsIn
                .Concat(managerGameWeekState.Opponents.SelectMany(o => o.SubsIn)).ToList(),
            true
        );

        var trendyBitchOutPoints = GetTrendyBitchPoints(
            managerGameWeekState.SubsOut.ToList(),
            managerGameWeekState.SubsOut
                .Concat(managerGameWeekState.Opponents.SelectMany(o => o.SubsOut)).ToList(),
            false
        );

        points += trendyBitchInPoints.Points + trendyBitchOutPoints.Points;

        return new RulePoints(points,$"{trendyBitchInPoints.Reasoning} {trendyBitchOutPoints.Reasoning}");
    }

    private RulePoints GetTrendyBitchPoints(IList<Sub> userSubs, IList<Sub> allSubs, bool tradeIn)
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
            return new RulePoints(-1, $"Traded {tradedInString} most popular player: {player.FirstName} {player.Surname}.");
        }

        return new RulePoints(0, null);
    }
}
