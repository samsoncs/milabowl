using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class SellIn : MilaRule
{
    protected override string ShortName => "$In";
    protected override string Description =>
        "Receive 1 point if your transfers in outperform your transfers out.";

    protected override RulePoints CalculatePoints(ManagerGameWeekState userGameWeek)
    {
        var sumPointsPlayersIn = userGameWeek.TransfersIn.Sum(pe => pe.TotalPoints);
        var sumPointsPlayersOut = userGameWeek.TransfersOut.Sum(pe => pe.TotalPoints);
        var points = sumPointsPlayersIn > sumPointsPlayersOut ? 1 : 0;
        return new RulePoints(
            points,
            $"Transfers in: {GetSubsString(userGameWeek.TransfersIn)} - Transfers out: {GetSubsString(userGameWeek.TransfersOut)}"
        );
    }

    private string GetSubsString(IReadOnlyList<Transfer> lineup)
    {
        return string.Join(",", lineup.Select(s => $"{s.FirstName} {s.Surname} ({s.TotalPoints})"));
    }
}
