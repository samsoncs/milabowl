using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class SixtyNineSub : MilaRule
{
    protected override string ShortName => "69Sub";

    protected override decimal CalculatePoints(MilaGameWeekState userGameWeek)
    {
        var cap = userGameWeek.Lineup.First(pe => pe.IsCaptain);
        return userGameWeek.Lineup.Any(pe => pe.Minutes == 69) ? 2.69m * cap.Multiplier : 0;
    }
}
