using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class SellIn : MilaRule
{
    protected override string ShortName => "$In";
    protected override string Description => "Receive 1 point if your subs in outperform your subs out.";

    protected override RulePoints CalculatePoints(ManagerGameWeekState userGameWeek)
    {
        var sumPointsPlayersIn = userGameWeek.SubsIn.Sum(pe => pe.TotalPoints);
        var sumPointsPlayersOut = userGameWeek.SubsOut.Sum(pe => pe.TotalPoints);
        var points = sumPointsPlayersIn > sumPointsPlayersOut ? 1 : 0;
        return new RulePoints(points, $"Subbed in: {GetSubsString(userGameWeek.SubsIn)} - Subbed out: {GetSubsString(userGameWeek.SubsOut)}");
    }

    private string GetSubsString(IReadOnlyList<Sub> lineup)
    {
        return string.Join(",", lineup.Select(s => $"{s.FirstName} {s.Surname} ({s.TotalPoints})"));
    }
}
