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
        $"""
            Bomb Tiers & Points
            - 🧨 Dynamite ({BombHelper.GetBombPoints(
                BombTier.Dynamite
            )} pts): 0-3 weeks since last explosion
            - 💣 Bomb ({BombHelper.GetBombPoints(
                BombTier.Dynamite
            )} pts): 4-6 weeks since last explosion
            - ☢️ Nuke ({BombHelper.GetBombPoints(
                BombTier.Dynamite
            )} pts): 7+ weeks since last explosion

            Explosion Damage
            - Bomb holder: Takes full damage when bomb explodes
            - Collateral damage: Bomb and Nuke tiers deal half damage to collateral targets
            - Collateral targets: Players whose captain matches the bomb holder's vice-captain

            Bomb Throwing
            Bomb is thrown when the holder:
            1. Wins H2H match: Bomb goes to their H2H opponent
            2. Uses a chip (except "manager"): Bomb goes to highest scorer that round
               - If tied for highest score, bomb is not thrown

            Bomb Diffusal
            - Diffusal kits: Awarded to players scoring 100+ points
            - Diffusing: If bomb holder has a kit when bomb should explode, they diffuse it for +6.9 pts
            - No damage: Nobody takes damage when bomb is diffused

            Explosion Schedule
            Bomb explodes on predetermined random gameweeks (7 total across the season)
            """;

    protected override RulePoints CalculatePoints(ManagerGameWeekState userGameWeek)
    {
        var bombState = _bombState.CalcBombStateForGw(userGameWeek);
        var bombPointValue = BombHelper.GetBombPoints(bombState.BombTier);

        var bombPoints = 0m;
        var reason = "";
        if (UserHoldsExplodingBomb(bombState, userGameWeek.User.EntryId))
        {
            bombPoints = bombPointValue;
            reason = $"Holding an exploding {bombState.BombTier}, {bombPoints} pts.";
        }

        if (UserIsCollateralOfExplodingBomb(bombState, userGameWeek.User.EntryId))
        {
            bombPoints = bombPointValue / 2;
            reason =
                $"Hit by collateral of an exploding {bombState.BombTier} bomb, {bombPoints} pts.";
        }

        if (UserHoldsDiffusedBomb(bombState, userGameWeek.User.EntryId))
        {
            bombPoints = 6.9m;
            reason = $"Diffused a {bombState.BombTier}, +{bombPoints} pts.";
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

    private bool UserHoldsDiffusedBomb(ManagerBombState bombState, int userEntryId)
    {
        return bombState.BombState == BombStateEnum.Diffused
            && bombState.BombHolder.FantasyManagerId == userEntryId;
    }
}
