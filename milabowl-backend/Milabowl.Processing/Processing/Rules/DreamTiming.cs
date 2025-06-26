using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class DreamTiming: MilaRule
{
    protected override string ShortName => "DrTm";

    protected override string Description =>
        "Receive 1.5 points if you sub in a player that is in dream team.";

    protected override RulePoints CalculatePoints(MilaGameWeekState userGameWeek)
    {
        var subsIn = userGameWeek.SubsIn.Select(u => u.FantasyPlayerEventId).ToList();
        var subsInDreamTeam =
            userGameWeek.Lineup.Where(l =>
                l.InDreamteam && subsIn.Contains(l.FantasyPlayerEventId))
                .ToList();
        var points = subsInDreamTeam.Any()
            ? 1.5m : 0;

        return new RulePoints(points, $"Subs in dream team: {string.Join(",", subsInDreamTeam.Select(s => $"{s.FirstName} {s.Surname}"))}");
    }
}
