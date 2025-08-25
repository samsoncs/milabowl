using Milabowl.Processing.Processing.BombState.Models;

namespace Milabowl.Processing.Processing.BombState.BombEventGenerators;

public class BombDiffusedEventGenerator : IBombEventGenerator
{
    public bool IsApplicable(ManagerBombState bombState)
    {
        return bombState.BombState == BombStateEnum.Diffused;
    }

    public BombHistoryRow GetRow(ManagerBombState bombState)
    {
        return new BombHistoryRow(
            $"{BombHelper.GetCapitalizedTierName(bombState.BombTier)} diffused by **{bombState.BombHolder.ManagerName}**. **(+6.9 pts!)**",
            $"{BombEmoji.GetBombEmoji(bombState.BombTier)}{BombEmoji.Diffused}",
            BombEventRowSeverity.Success
        );
    }
}
