using Milabowl.Processing.DataImport;

namespace Milabowl.Processing.Processing.Rules;

public class BenchFail : IMilaRule
{
    public MilaRuleResult Calculate(UserGameWeek userGameWeek)
    {
        var points =
            (decimal)
                Math.Floor(
                    userGameWeek.Lineup.Where(pe => pe.Multiplier == 0).Sum(pe => pe.TotalPoints)
                        / 5.0
                ) * -1;
        return new MilaRuleResult("BenchFail", "BF", points);
    }
}
