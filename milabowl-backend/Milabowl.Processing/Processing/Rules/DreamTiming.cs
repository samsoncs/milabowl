using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class DreamTiming: MilaRule
{
    protected override string ShortName => "DrTm";
    protected override decimal CalculatePoints(MilaGameWeekState userGameWeek)
    {
        var subsIn = userGameWeek.User.SubsIn.Select(u => u.FantasyPlayerEventId).ToList();
        if(userGameWeek.User.Lineup.Any(l => l.InDreamteam && subsIn.Contains(l.FantasyPlayerEventId)))
        {
            return 1.5m;
        }

        return 0;
    }
}
