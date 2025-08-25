using Milabowl.Processing.Processing.BombState.Models;

namespace Milabowl.Processing.Processing.BombState.BombEventGenerators;

public class BombExplodedEventGenerator : IBombEventGenerator
{
    public bool IsApplicable(ManagerBombState bombState)
    {
        return bombState.BombState == BombStateEnum.Exploded;
    }

    public BombHistoryRow GetRow(ManagerBombState bombState)
    {
        return new BombHistoryRow(
            $"{BombHelper.GetCapitalizedTierName(bombState.BombTier)} exploded on **{bombState.BombHolder.ManagerName}**. **({BombHelper.GetBombPoints(bombState.BombTier)} pts!)**",
            $"{BombEmoji.GetBombEmoji(bombState.BombTier)}{BombEmoji.Exploded}",
            BombEventRowSeverity.Danger
        );
    }
}
