using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class MinusIsPlus : MilaRule
{
    public override string ShortName => "MiP";

    protected override decimal CalculatePoints(UserGameWeek userGameWeek)
    {
        return userGameWeek
            .Lineup.Where(pe => pe.TotalPoints < 0)
            .Sum(pe => pe.TotalPoints * -1 * pe.Multiplier);
    }
}
