﻿using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class HeadToHeadMeta : MilaRule
{
    protected override string ShortName => "H2H";
    protected override string Description => "Receive 2 points if you beat your H2H opponent with less than 3 points.";

    protected override RulePoints CalculatePoints(MilaGameWeekState userGameWeek)
    {
        var scoreDiff =
            userGameWeek.User.HeadToHead.CurrentUser.Points
            - userGameWeek.User.HeadToHead.Opponent.Points;

        var points =
            userGameWeek.User.HeadToHead.CurrentUser.DidWin && scoreDiff is > 0 and <= 2 ? 2 : 0;

        return new RulePoints(points, null);
    }
}
