using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class SixtyNine : MilaRule
{
    protected override string ShortName => "69";

    protected override decimal CalculatePoints(UserGameWeek userGameWeek)
    {
        var totalTeamScore = userGameWeek.Lineup.Sum(pe => pe.TotalPoints * pe.Multiplier);
        return totalTeamScore == 69 ? 6.9m : 0;
    }
}
