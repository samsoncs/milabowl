using Milabowl.Processing.DataImport;
using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class HeadToHeadMeta : IMilaRule
{
    public MilaRuleResult Calculate(UserGameWeek userGameWeek)
    {
        var scoreDiff =
            userGameWeek.HeadToHead.CurrentUser.Points - userGameWeek.HeadToHead.Opponent.Points;

        var points =
            userGameWeek.HeadToHead.CurrentUser.DidWin && scoreDiff is > 0 and <= 2 ? 2 : 0;

        return new MilaRuleResult("HeadToHeadMeta", "H2H", points);
    }
}
