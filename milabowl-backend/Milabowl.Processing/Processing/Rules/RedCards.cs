using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class RedCards : MilaRule
{
    public override string ShortName => "RC";

    protected override decimal CalculatePoints(UserGameWeek userGameWeek)
    {
        return userGameWeek.Lineup.Where(pe => pe.RedCards == 1).Sum(pe => pe.Multiplier);
    }
}
