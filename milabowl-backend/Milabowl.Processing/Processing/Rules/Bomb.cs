using Milabowl.Processing.DataImport.Models;
using Milabowl.Processing.Processing.BombState;
using Milabowl.Processing.Processing.BombState.Models;

namespace Milabowl.Processing.Processing.Rules;

public class Bomb : MilaRule
{
    private readonly IBombState _bombState;

    public Bomb(IBombState bombState)
    {
        _bombState = bombState;
    }

    protected override string ShortName => "Bmb";
    protected override string Description =>
        "The bomb explodes 7 times during a season. The person holding the bomb upon explosion receives -5 points. To pass the bomb along, you must wither win your H2H match or use a chip. If you win H2H, the bomb is passed to the H2H opponent. If a chip is used the bomb is passed to the top scoring FPL manager this week (that is not holding the bomb).";

    protected override RulePoints CalculatePoints(ManagerGameWeekState userGameWeek)
    {
        var bombState = _bombState.CalcBombStateForGw(userGameWeek);
        var bombPointValue = bombState.BombTier switch
        {
            BombTier.Dynamite => -2m,
            BombTier.Bomb => -4m,
            BombTier.Nuke => -6m,
            _ => 0m,
        };

        var bombPoints = 0m;
        var reason = "";
        if (UserHoldsExplodingBomb(bombState, userGameWeek.User.EntryId))
        {
            bombPoints = bombPointValue;
            reason = $"Holding an exploding {bombState.BombTier} bomb, - {bombPoints} pts.";
        }

        if (UserIsCollateralOfExplodingBomb(bombState, userGameWeek.User.EntryId))
        {
            bombPoints = bombPointValue / 2;
            reason =
                $"Hit by collateral of an exploding {bombState.BombTier} bomb, - {bombPoints} pts.";
        }

        return new RulePoints(bombPoints, reason);
    }

    private bool UserHoldsExplodingBomb(ManagerBombState bombState, int userEntryId)
    {
        return bombState.BombState == BombStateEnum.Exploded
            && bombState.BombHolder.FantasyManagerId == userEntryId;
    }

    private bool UserIsCollateralOfExplodingBomb(ManagerBombState bombState, int userEntryId)
    {
        return bombState
                is { BombState: BombStateEnum.Exploded, BombTier: BombTier.Bomb or BombTier.Nuke }
            && bombState.CollateralTargets.Select(b => b.FantasyManagerId).Contains(userEntryId);
    }
}
