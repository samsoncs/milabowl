using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class HeadToHeadMeta : MilaRule
{
    protected override string ShortName => "H2H";

    protected override decimal CalculatePoints(MilaGameWeekState userGameWeek)
    {
        var scoreDiff =
            userGameWeek.User.HeadToHead.CurrentUser.Points
            - userGameWeek.User.HeadToHead.Opponent.Points;

        var points =
            userGameWeek.User.HeadToHead.CurrentUser.DidWin && scoreDiff is > 0 and <= 2 ? 2 : 0;

        return points;
    }
}
