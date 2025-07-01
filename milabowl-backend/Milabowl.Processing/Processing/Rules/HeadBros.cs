using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class HeadBros : MilaRule
{
    protected override string ShortName => "HdBrs";
    protected override string Description =>
        "Score 2.69 points if you and your head bro has the highest total score that round. If tied all ties receive 2.69 pts.";

    protected override RulePoints CalculatePoints(ManagerGameWeekState userGameWeek)
    {
        var currentHeadBrosPoints =
            userGameWeek.HeadToHead.CurrentUser.Points + userGameWeek.HeadToHead.Opponent.Points;
        var maxOpponentsHeadBroPoints = userGameWeek.Opponents.Max(o =>
            o.HeadToHead.CurrentUser.Points + o.HeadToHead.Opponent.Points
        );
        var points = currentHeadBrosPoints >= maxOpponentsHeadBroPoints ? 2.69m : 0;

        return new RulePoints(points, null);
    }
}
