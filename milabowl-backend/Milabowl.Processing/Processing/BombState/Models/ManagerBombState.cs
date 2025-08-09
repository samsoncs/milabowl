namespace Milabowl.Processing.Processing.BombState.Models;

public record ManagerBombState(
    BombStateEnum BombState,
    BombManager BombHolder,
    BombManager? BombThrower,
    BombTier BombTier,
    int WeeksSinceLastExplosion,
    IList<BombManager> CollateralTargets,
    string? CollateralTargetPlayerName,
    IList<BombManager> BombDiffusalKits
);
