using Milabowl.Processing.Processing.BombState.Models;

namespace Milabowl.Processing.Processing.BombState.BombEventGenerators;

public class BombHeld : IBombEventGenerator
{
    public bool CanGenerate(ManagerBombState bombState)
    {
        return bombState.BombState != BombStateEnum.Diffused
            && bombState.BombState != BombStateEnum.Exploded;
    }

    public BombHistoryRow Generate(ManagerBombState bombState)
    {
        var flavorText = bombState.BombThrower != null ? " caught the" : " is holding the";
        return new BombHistoryRow(
            $"**{bombState.BombHolder.ManagerName}** {flavorText} {BombHelper.GetTierName(bombState.BombTier)}.",
            $"{BombHelper.GetBombEmoji(bombState.BombTier)}{BombHelper.Holding}",
            BombEventRowSeverity.None
        );
    }

    public IList<BombStateDisplayEmoji> GenerateDisplayEmojis(ManagerBombState bombState)
    {
        return
        [
            new BombStateDisplayEmoji(
                bombState.BombHolder.FantasyManagerId,
                $"{BombHelper.GetBombEmoji(bombState.BombTier)}"
            ),
        ];
    }
}
