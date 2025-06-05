using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class HeadBros: MilaRule
{
    protected override string ShortName => "HdBrs";
    protected override decimal CalculatePoints(MilaGameWeekState userGameWeek)
    {
        var currentHeadBrosPoints =  userGameWeek.User.HeadToHead.CurrentUser.Points
                           + userGameWeek.User.HeadToHead.Opponent.Points;
        var maxOpponentsHeadBroPoints = userGameWeek.Opponents.Max(o => o.HeadToHead.CurrentUser.Points + o.HeadToHead.Opponent.Points);

        if (currentHeadBrosPoints >= maxOpponentsHeadBroPoints)
        {
            return 2.69m;
        }

        return 0;
    }
}
