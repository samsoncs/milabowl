using Milabowl.Processing.Processing.BombState.Models;

namespace Milabowl.Processing.Processing.BombState.BombEventGenerators;

public class BombHeldEventGenerator : IBombEventGenerator
{
    public bool IsApplicable(ManagerBombState bombState)
    {
        return bombState.BombState != BombStateEnum.Diffused
               && bombState.BombState != BombStateEnum.Exploded;
    }

    public BombHistoryRow GetRow(ManagerBombState bombState)
    {
        var flavorText = bombState.BombThrower != null
            ? " caught the"
            : " is holding the";
        return new BombHistoryRow(
            $"**{bombState.BombHolder.ManagerName}** {flavorText} {BombHelper.GetTierName(bombState.BombTier)}.",
            $"{BombEmoji.GetBombEmoji(bombState.BombTier)}{BombEmoji.Holding}",
            BombEventRowSeverity.None
        );
    }
}
