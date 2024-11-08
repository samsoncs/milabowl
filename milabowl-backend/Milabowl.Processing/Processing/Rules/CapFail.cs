﻿using Milabowl.Processing.DataImport;

namespace Milabowl.Processing.Processing.Rules;

public class CapFailScore: IMilaRule
{
    public MilaRuleResult Calculate(UserGameWeek userGameWeek)
    {
        return new MilaRuleResult(
            "CapFail",
            "CF",
            userGameWeek.Lineup.Any(pe => pe is { IsCaptain: true, TotalPoints: < 5 }) ? -1 : 0
        );

    }
}
