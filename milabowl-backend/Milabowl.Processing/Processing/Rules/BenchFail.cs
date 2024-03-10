using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class BenchFail : MilaRule
{
    protected override string ShortName => "BF";

    protected override decimal CalculatePoints(UserGameWeek userGameWeek)
    {
        var points =
            (decimal)
                Math.Floor(
                    userGameWeek.Lineup.Where(pe => pe.Multiplier == 0).Sum(pe => pe.TotalPoints)
                        / 5.0
                ) * -1;
        return points;
    }
}
