using Milabowl.Processing.Processing.BombState.Models;

namespace Milabowl.Processing.Processing.BombState.BombEventGenerators;

public class BombDiffusalEarned : IBombEventGenerator
{
    public bool CanGenerate(ManagerBombState bombState)
    {
        return bombState.BombDiffusalKits.Any();
    }

    public BombHistoryRow Generate(ManagerBombState bombState)
    {
        return new BombHistoryRow(
            $"**{string.Join(", ", bombState.BombDiffusalKits.Select(c => c.ManagerName))}** earned a diffusal kit!",
            $"{BombHelper.DiffusalKit}",
            BombEventRowSeverity.Success
        );
    }

    public IList<BombStateDisplayEmoji> GenerateDisplayEmojis(ManagerBombState bombState)
    {
        return bombState
            .BombDiffusalKits.Select(target => new BombStateDisplayEmoji(
                target.FantasyManagerId,
                target.UserId,
                $"{BombHelper.DiffusalKit}"
            ))
            .ToList();
    }
}
