using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class MinusIsPlus : MilaRule
{
    protected override string ShortName => "MiP";

    protected override decimal CalculatePoints(MilaGameWeekState userGameWeek)
    {
        return userGameWeek
            .User.Lineup.Where(pe => pe.TotalPoints < 0)
            .Sum(pe => pe.TotalPoints * -1 * pe.Multiplier);
    }
}
