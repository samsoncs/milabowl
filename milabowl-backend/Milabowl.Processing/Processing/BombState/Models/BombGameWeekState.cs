namespace Milabowl.Processing.Processing.BombState.Models;

public record BombGameWeekState(
    int GameWeek,
    BombStateEnum BombState,
    BombManager BombHolder,
    BombManager? BombThrower,
    BombTier BombTier,
    int WeeksSinceLastExplosion,
    IList<BombManager> CollateralTargets,
    string? CollateralTargetPlayerName,
    IList<BombManager> BombDiffusalKits
);