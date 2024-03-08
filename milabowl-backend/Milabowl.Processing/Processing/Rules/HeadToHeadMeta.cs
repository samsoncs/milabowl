using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class HeadToHeadMeta : MilaRule
{
    public override string ShortName => "H2H";

    protected override decimal CalculatePoints(UserGameWeek userGameWeek)
    {
        var scoreDiff =
            userGameWeek.HeadToHead.CurrentUser.Points - userGameWeek.HeadToHead.Opponent.Points;

        var points =
            userGameWeek.HeadToHead.CurrentUser.DidWin && scoreDiff is > 0 and <= 2 ? 2 : 0;

        return points;
    }
}
