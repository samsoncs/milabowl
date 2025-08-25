using Milabowl.Processing.Processing.BombState.Models;

namespace Milabowl.Processing.Processing.BombState.BombEventGenerators;

public class BombExploded : IBombEventGenerator
{
    public bool CanGenerate(ManagerBombState bombState)
    {
        return bombState.BombState == BombStateEnum.Exploded;
    }

    public BombHistoryRow Generate(ManagerBombState bombState)
    {
        return new BombHistoryRow(
            $"{BombHelper.GetCapitalizedTierName(bombState.BombTier)} exploded on **{bombState.BombHolder.ManagerName}**. **({BombHelper.GetBombPoints(bombState.BombTier)} pts!)**",
            $"{BombHelper.GetBombEmoji(bombState.BombTier)}{BombHelper.Exploded}",
            BombEventRowSeverity.Danger
        );
    }

    public IList<BombStateDisplayEmoji> GenerateDisplayEmojis(ManagerBombState bombState)
    {
        return
        [
            new BombStateDisplayEmoji(bombState.BombHolder.FantasyManagerId, BombHelper.Exploded),
        ];
    }
}
