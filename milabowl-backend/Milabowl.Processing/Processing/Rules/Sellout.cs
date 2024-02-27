using Milabowl.Processing.DataImport;

namespace Milabowl.Processing.Processing.Rules;

public class Sellout
{
    public MilaRuleResult Calculate(UserGameWeek userGameWeek)
    {
        // if(!subsOut.Any() || !subsIn.Any())
        // {
        //     return 0;
        // }
        //
        // var sumPointsPlayersIn = subsIn.Sum(p => p.PlayerEvents.FirstOrDefault(pe => pe?.Event?.GameWeek == gameWeek)?.TotalPoints ?? 0);
        // var sumPointsPlayersOut = subsOut.Sum(p => p.PlayerEvents.FirstOrDefault(pe => pe?.Event?.GameWeek == gameWeek)?.TotalPoints ?? 0);
        //
        // return sumPointsPlayersOut > sumPointsPlayersIn ? -2 : 0;

        // return new MilaRuleResult(
        //     "$ellout",
        //     "$O",
        //     );

        throw new NotImplementedException("Need subs for sellout");
    }
}
