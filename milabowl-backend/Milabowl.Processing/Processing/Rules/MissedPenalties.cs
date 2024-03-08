using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class MissedPenalties : MilaRule
{
    public override string ShortName => "MP";

    protected override decimal CalculatePoints(UserGameWeek userGameWeek)
    {
        return userGameWeek.Lineup.Sum(p => (p.PenaltiesMissed > 0 ? 1.69m : 0) * p.Multiplier);
    }
}
