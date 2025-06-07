using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class CapFailScore : MilaRule
{
    protected override string ShortName => "CF";
    protected override string Description => "Receive -1 penalty points if captain scores less than 5 points.";

    protected override RulePoints CalculatePoints(MilaGameWeekState userGameWeek)
    {
        var captain = userGameWeek.User.Lineup.First(pe => pe.IsCaptain);
        var points = captain.TotalPoints < 5
            ? -1
            : 0;
        return new RulePoints(points,$"Captain {captain.FirstName} {captain.Surname} scored {captain.TotalPoints} pts.");
    }
}
