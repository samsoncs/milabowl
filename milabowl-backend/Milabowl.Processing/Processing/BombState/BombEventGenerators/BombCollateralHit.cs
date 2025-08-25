using Milabowl.Processing.Processing.BombState.Models;

namespace Milabowl.Processing.Processing.BombState.BombEventGenerators;

public class BombCollateralHit : IBombEventGenerator
{
    public bool CanGenerate(ManagerBombState bombState)
    {
        return bombState.BombState == BombStateEnum.Exploded
            && bombState.BombTier != BombTier.Dynamite
            && bombState.CollateralTargets.Any();
    }

    public BombHistoryRow Generate(ManagerBombState bombState)
    {
        return new BombHistoryRow(
            $"**{string.Join(", ", bombState.CollateralTargets.Select(c => c.ManagerName))}** was targeted by VC **{bombState.CollateralTargetPlayerName}** and hit by collateral. **({BombHelper.GetBombPoints(bombState.BombTier) / 2} pts!)**",
            $"{BombHelper.GetBombEmoji(bombState.BombTier)}{BombHelper.Collateral}",
            BombEventRowSeverity.Danger
        );
    }

    public IList<BombStateDisplayEmoji> GenerateDisplayEmojis(ManagerBombState bombState)
    {
        return bombState
            .CollateralTargets.Select(target => new BombStateDisplayEmoji(
                target.FantasyManagerId,
                $"{BombHelper.Collateral}{BombHelper.Exploded}"
            ))
            .ToList();
    }
}
