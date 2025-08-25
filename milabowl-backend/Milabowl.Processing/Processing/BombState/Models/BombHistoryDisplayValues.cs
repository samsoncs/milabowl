namespace Milabowl.Processing.Processing.BombState.Models;

public record BombHistoryDisplayValues(
    IDictionary<int, List<BombHistoryRow>> BombHistoryByGameWeek,
    IList<BombStateDisplayEmoji> CurrentRoundEmojis,
    CurrentStateValues CurrentState
);
