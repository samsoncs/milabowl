using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class BlackWidow : MilaRule
{
    protected override string ShortName => "BW";
    protected override string Description =>
        "Win or loose points based on hits taken. Each gameweek a 50/50 roll determines whether even or odd number of opponents with hits gives positive points. Multiply number of hits by 0.42 (or -0.42). If no hits, no points.";

    protected override RulePoints CalculatePoints(ManagerGameWeekState userGameWeek)
    {
        // GW1-5 use the old logic, GW6 onwards use the new logic.
        if (userGameWeek.Event.GameWeek < 6)
            return CalculatePointsOld(userGameWeek);

        var hits = Math.Clamp(GetHits(userGameWeek.TransferCost), 0, 10);
        if (hits == 0)
            return new RulePoints(0, null);

        var opponentsWithHits = userGameWeek.Opponents.Sum(o =>
            GetHits(o.TransferCost) > 0 ? 1 : 0
        );

        var evenWins = new Random(userGameWeek.Event.GameWeek).Next(2) == 0;
        var isPositive = evenWins ? opponentsWithHits % 2 == 0 : opponentsWithHits % 2 != 0;

        return new RulePoints(hits * (isPositive ? 0.42m : -0.42m), null);
    }

    private RulePoints CalculatePointsOld(ManagerGameWeekState userGameWeek)
    {
        var hits = Math.Clamp(GetHits(userGameWeek.TransferCost), 0, 2);
        var opponentsWithHits = userGameWeek.Opponents.Sum(o =>
            GetHits(o.TransferCost) > 0 ? 1 : 0
        );

        if (hits > 0 && opponentsWithHits % 2 == 0)
        {
            return new RulePoints(hits * 4.2m, null);
        }
        if (hits > 0)
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
