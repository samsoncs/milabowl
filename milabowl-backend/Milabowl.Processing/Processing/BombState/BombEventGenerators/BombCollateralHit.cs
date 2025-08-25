using Milabowl.Processing.Processing.BombState.Models;

namespace Milabowl.Processing.Processing.BombState.BombEventGenerators;

public class BombCollateral : IBombEventGenerator
{
    public bool IsApplicable(ManagerBombState bombState)
    {
        return bombState.BombState != BombStateEnum.Diffused
               && bombState.BombState != BombStateEnum.Exploded;
    }

    public BombHistoryRow GetRow(ManagerBombState bombState)
    {
        return new BombHistoryRow(
            $"**{string.Join(", ", bombState.CollateralTargets.Select(c => c.ManagerName))}** was targeted by VC **{bombState.CollateralTargetPlayerName}** and hit by collateral. **({BombHelper.GetBombPoints(bombState.BombTier) / 2} pts!)**",
            $"{BombEmoji.GetBombEmoji(bombState.BombTier)}{BombEmoji.Collateral}",
            BombEventRowSeverity.Danger
        );
    }
}
