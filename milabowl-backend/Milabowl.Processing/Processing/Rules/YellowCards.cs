using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class YellowCards : MilaRule
{
    public override string ShortName => "YC";

    protected override decimal CalculatePoints(UserGameWeek userGameWeek)
    {
        return userGameWeek.Lineup.Where(pe => pe.YellowCards == 1).Sum(pe => pe.Multiplier);
    }
}
