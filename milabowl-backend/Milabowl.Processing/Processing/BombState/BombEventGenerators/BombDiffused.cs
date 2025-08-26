using Milabowl.Processing.Processing.BombState.Models;

namespace Milabowl.Processing.Processing.BombState.BombEventGenerators;

public class BombDiffused : IBombEventGenerator
{
    public bool CanGenerate(ManagerBombState bombState)
    {
        return bombState.BombState == BombStateEnum.Diffused;
    }

    public BombHistoryRow Generate(ManagerBombState bombState)
    {
        return new BombHistoryRow(
            $"{BombHelper.GetCapitalizedTierName(bombState.BombTier)} diffused by **{bombState.BombHolder.ManagerName}**. **(+6.9 pts!)**",
            $"{BombHelper.GetBombEmoji(bombState.BombTier)}{BombHelper.Diffused}",
            BombEventRowSeverity.Success
        );
    }

    public IList<BombStateDisplayEmoji> GenerateDisplayEmojis(ManagerBombState bombState)
    {
        return
        [
            new BombStateDisplayEmoji(
                bombState.BombHolder.FantasyManagerId,
                bombState.BombHolder.UserId,
                BombHelper.Diffused
            ),
        ];
    }
}
