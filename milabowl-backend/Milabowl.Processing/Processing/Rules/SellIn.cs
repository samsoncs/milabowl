using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class SellIn : MilaRule
{
    protected override string ShortName => "$In";

    protected override decimal CalculatePoints(MilaGameWeekState userGameWeek)
    {
        var sumPointsPlayersIn = userGameWeek.User.SubsIn.Sum(pe => pe.TotalPoints);
        var sumPointsPlayersOut = userGameWeek.User.SubsOut.Sum(pe => pe.TotalPoints);
        return sumPointsPlayersIn > sumPointsPlayersOut ? 1 : 0;
    }
}
