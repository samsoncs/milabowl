using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class YellowCards : MilaRule
{
    protected override string ShortName => "YC";

    protected override decimal CalculatePoints(MilaGameWeekState userGameWeek)
    {
        return userGameWeek.User.Lineup.Where(pe => pe.YellowCards == 1).Sum(pe => pe.Multiplier);
    }
}
