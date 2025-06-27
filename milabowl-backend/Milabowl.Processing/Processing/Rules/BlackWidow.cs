using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class BlackWidow: MilaRule
{
    protected override string ShortName => "BW";
    protected override string Description => "Win or loose points based on hits taken. If there are an even number of opponents with hits multiply number of hits by 0.42, otherwise multiply by -0.42. If no hits, no points.";
    protected override RulePoints CalculatePoints(ManagerGameWeekState userGameWeek)
    {
        var hits = GetHits(userGameWeek.TransferCost);
        var opponentsWithHits = userGameWeek.Opponents.Sum(o => GetHits(o.TransferCost) > 0 ? 1 : 0);

        if (hits > 0 && opponentsWithHits % 2 == 0)
        {
            return new RulePoints(hits * 4.2m, null);
        }
        if(hits > 0)
        {
            return new RulePoints(hits * -4.2m, null);
        }

        return new RulePoints(0, null);
    }

    private int GetHits(int transferCost)
    {
        return (int)Math.Floor(transferCost / 4.0m);
    }
};
