using Milabowl.Processing.DataImport;

namespace Milabowl.Processing.Processing.Rules;

public class Sellout : IMilaRule
{
    public MilaRuleResult Calculate(UserGameWeek userGameWeek)
    {
        var sumPointsPlayersIn = userGameWeek.SubsIn.Sum(pe => pe.Multiplier * pe.TotalPoints);
        var sumPointsPlayersOut = userGameWeek.SubsOut.Sum(pe => pe.Multiplier * pe.TotalPoints);

        return new MilaRuleResult(
            "$ellout",
            "$O",
            sumPointsPlayersOut > sumPointsPlayersIn ? -2 : 0
        );
    }
}
