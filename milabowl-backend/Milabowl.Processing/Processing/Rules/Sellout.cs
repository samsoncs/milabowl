using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class Sellout : MilaRule
{
    protected override string ShortName => "$0";

    protected override decimal CalculatePoints(MilaGameWeekState userGameWeek)
    {
        var sumPointsPlayersIn = userGameWeek.User.SubsIn.Sum(pe => pe.Multiplier * pe.TotalPoints);
        var sumPointsPlayersOut = userGameWeek.User.SubsOut.Sum(pe =>
            pe.Multiplier * pe.TotalPoints
        );
        return sumPointsPlayersOut > sumPointsPlayersIn ? -2 : 0;
    }
}
