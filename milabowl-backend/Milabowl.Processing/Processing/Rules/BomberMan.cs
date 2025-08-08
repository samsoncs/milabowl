using Milabowl.Processing.DataImport.Models;
using Milabowl.Processing.Processing.BombState;
using Milabowl.Processing.Processing.BombState.Models;

namespace Milabowl.Processing.Processing.Rules;

public class BomberMan : MilaRule
{
    private readonly IBombState _bombState;

    public BomberMan(IBombState bombState)
    {
        _bombState = bombState;
    }

    protected override string ShortName => "BmbMn";
    protected override string Description => "Get 5 points if you throw an exploding bomb.";

    protected override RulePoints CalculatePoints(ManagerGameWeekState userGameWeek)
    {
        var bombState = _bombState.CalcBombStateForGw(userGameWeek);

        var bombThrowerPoints =
            bombState.BombState == BombStateEnum.Exploded
            && bombState.BombThrower is not null
            && bombState.BombThrower.FantasyManagerId == userGameWeek.User.EntryId
                ? 5m
                : 0;

        return new RulePoints(bombThrowerPoints, null);
    }
}
