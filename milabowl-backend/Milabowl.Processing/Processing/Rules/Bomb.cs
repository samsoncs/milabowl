using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class Bomb: MilaRule
{
    private readonly IBombState _bombState;

    public Bomb(IBombState bombState)
    {
        _bombState = bombState;
    }

    protected override string ShortName => "Bmb";
    protected override string Description => "The bomb explodes 7 times during a season. The person holding the bomb upon explosion receives -5 points. To pass the bomb along, you must wither win your H2H match or use a chip. If you win H2H, the bomb is passed to the H2H opponent. If a chip is used the bomb is passed to the top scoring FPL manager this week (that is not holding the bomb).";
    protected override RulePoints CalculatePoints(ManagerGameWeekState userGameWeek)
    {
        var bombState = _bombState.CalcBombStateForGw(userGameWeek);
        var bombPoints =
            bombState.BombState == BombStateEnum.Exploded &&
            bombState.BombHolder.FantasyManagerId == userGameWeek.User.EntryId
                ? -5m
                : 0;

        return new RulePoints(bombPoints, null);
    }
}
