using Milabowl.Processing.Processing.BombState.Models;

namespace Milabowl.Processing.Processing.BombState.BombEventGenerators;

public class BombThrownEventGenerator: IBombEventGenerator
{
    public bool IsApplicable(ManagerBombState bombState)
    {
        return bombState.BombThrower is not null;
    }

    public BombHistoryRow GetRow(ManagerBombState bombState)
    {
        return new BombHistoryRow(
            $"**{bombState.BombHolder.ManagerName}** threw the {BombHelper.GetTierName(bombState.BombTier)}.",
            $"{BombEmoji.Thrown}{BombEmoji.GetBombEmoji(bombState.BombTier)}",
            BombEventRowSeverity.Info
        );
    }
}
