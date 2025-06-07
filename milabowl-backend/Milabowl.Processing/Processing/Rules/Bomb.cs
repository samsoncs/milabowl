using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class Bomb: MilaRule
{
    private readonly BombState _bombState;

    public Bomb(BombState bombState)
    {
        _bombState = bombState;
    }

    protected override string ShortName => "Bmb";
    protected override string Description => "The bomb explodes 7 times during a season. The person holding the bomb upon explosion receives -5 points. To pass the bomb along, you must win your H2H match.";
    protected override RulePoints CalculatePoints(MilaGameWeekState userGameWeek)
    {
        var bombState = _bombState.CalcBombStateForGw(userGameWeek);
        var bombPoints =
            bombState.BombState == BombStateEnum.Exploded &&
            bombState.BombHolder.FantasyManagerId == userGameWeek.User.User.Id
                ? -5m
                : 0;

        return new RulePoints(bombPoints, null);
    }
}
