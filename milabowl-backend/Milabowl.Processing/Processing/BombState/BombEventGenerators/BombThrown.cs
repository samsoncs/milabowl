using Milabowl.Processing.Processing.BombState.Models;

namespace Milabowl.Processing.Processing.BombState.BombEventGenerators;

public class BombThrown : IBombEventGenerator
{
    public bool CanGenerate(ManagerBombState bombState)
    {
        return bombState.BombThrower is not null;
    }

    public BombHistoryRow Generate(ManagerBombState bombState)
    {
        return new BombHistoryRow(
            $"**{bombState.BombHolder.ManagerName}** threw the {BombHelper.GetTierName(bombState.BombTier)}.",
            $"{BombHelper.Thrown}{BombHelper.GetBombEmoji(bombState.BombTier)}",
            BombEventRowSeverity.None
        );
    }

    public IList<BombStateDisplayEmoji> GenerateDisplayEmojis(ManagerBombState bombState)
    {
        return
        [
            new BombStateDisplayEmoji(
                bombState.BombHolder.FantasyManagerId,
                bombState.BombHolder.UserId,
                BombHelper.Thrown
            ),
        ];
    }
}
