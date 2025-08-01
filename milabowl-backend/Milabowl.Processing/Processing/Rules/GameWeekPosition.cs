﻿using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class GameWeekPosition : MilaRule
{
    protected override string ShortName => "GW PS";
    protected override string Description =>
        "Score points based on game week ranking. Last place recieves 0 pts, which increases by 0.5 pr. rank.";

    protected override RulePoints CalculatePoints(ManagerGameWeekState userGameWeek)
    {
        var rulePoints = 0.0m;
        var iteration = 0.0m;
        var isSoleHighestScore = true;
        foreach (
            var grp in userGameWeek.Opponents.OrderBy(m => m.TotalScore).GroupBy(g => g.TotalScore)
        )
        {
            if (userGameWeek.TotalScore <= grp.Key)
            {
                rulePoints = iteration / 2.0m;
                isSoleHighestScore = false;
                break;
            }

            iteration++;
        }

        if (isSoleHighestScore)
        {
            rulePoints = iteration / 2;
        }

        return new RulePoints(rulePoints, null);
    }
}
