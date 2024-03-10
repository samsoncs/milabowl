using System.Collections.ObjectModel;

namespace Milabowl.Processing.DataImport.Models;

public record MilaGameWeekState
{
    public UserState User { get; }
    public IReadOnlyList<UserState> Opponents { get; }

    public MilaGameWeekState(UserState user, IList<UserState> opponents)
    {
        User = user;
        Opponents = opponents.Where(u => u.User.Id != user.User.Id).ToList().AsReadOnly();
    }
}

public class UserState
{
    public User User { get; }
    public Event Event { get; }
    public IReadOnlyList<PlayerEvent> Lineup { get; }
    public IReadOnlyList<PlayerEvent> SubsIn { get; }
    public IReadOnlyList<PlayerEvent> SubsOut { get; }
    public HeadToHead HeadToHead { get; }
    public UserState? PreviousGameWeek { get; }
    public IList<UserState> History { get; }
    public decimal TotalScore { get; }
    public string? ActiveChip { get; }

    public UserState(
        Event @event,
        HeadToHead headToHead,
        User user,
        IList<PlayerEvent> lineup,
        string? activeChip,
        IList<UserState> historicGameWeeks
    )
    {
        Event = @event;
        User = user;
        Lineup = lineup.AsReadOnly();
        HeadToHead = headToHead;
        historicGameWeeks = historicGameWeeks
            .Where(e => e.Event.GameWeek < Event.GameWeek)
            .ToList();
        PreviousGameWeek = historicGameWeeks.FirstOrDefault(h =>
            h.Event.GameWeek == @event.GameWeek - 1 && h.User.Id == User.Id
        );
        SubsIn = PreviousGameWeek is null
            ? new ReadOnlyCollection<PlayerEvent>([])
            : Lineup
                .Where(pe =>
                    PreviousGameWeek.Lineup.All(ipe =>
                        ipe.FantasyPlayerEventId != pe.FantasyPlayerEventId
                    )
                )
                .ToList()
                .AsReadOnly();
        SubsOut = PreviousGameWeek is null
            ? new ReadOnlyCollection<PlayerEvent>([])
            : PreviousGameWeek
                .Lineup.Where(pe =>
                    Lineup.All(ipe => ipe.FantasyPlayerEventId != pe.FantasyPlayerEventId)
                )
                .ToList()
                .AsReadOnly();
        History = historicGameWeeks.Where(h => h.User.Id == User.Id).ToList().AsReadOnly();
        ActiveChip = activeChip;
        TotalScore = lineup.Sum(l => l.TotalPoints * l.Multiplier);
        // var totalScore = Lineup.Sum(l => l.TotalPoints * l.Multiplier);
        // var cumulativeTotalScore = UserHistory.Sum(h => h.FplScores.TotalScore) + totalScore;
        // var avgCumulativeTotalScore = Math.Round(cumulativeTotalScore / Event.GameWeek, 2);
        // FplScores = new FplScores(totalScore, cumulativeTotalScore, avgCumulativeTotalScore);
    }

    // public void AddOpponentsForGameWeek(IList<UserState> userGameWeeks)
    // {
    //     Opponents = userGameWeeks.Where(u => u.User.Id != User.Id).ToList();
    // }

    // public MilaGameWeekState GetCalculationState()
    // {
    //     return new MilaGameWeekState(
    //         User,
    //         Event,
    //         Lineup.AsReadOnly(),
    //         SubsIn.AsReadOnly(),
    //         SubsOut.AsReadOnly(),
    //         Opponents.AsReadOnly(),
    //         HeadToHead,
    //         FplScores,
    //         MilaScores,
    //         PreviousGameWeek,
    //         UserHistory.AsReadOnly()
    //     );
    // }
    //
    // public void AddMilaRuleResults(List<MilaRuleResult> results)
    // {
    //     Rules = results.ToDictionary(
    //         k => k.RuleName,
    //         e => new RuleResult(e.Points, e.RuleShortName)
    //     );
    //     SetMilaScores(results);
    //
    //     var gwPosition =
    //         Opponents.Sum(u => u.FplScores.TotalScore > FplScores.TotalScore ? 1 : 0) + 1;
    //     var milaRank =
    //         Opponents.Sum(o =>
    //             o.MilaScores.CumulativeTotalMilaScore > MilaScores.CumulativeTotalMilaScore ? 1 : 0
    //         ) + 1;
    //     var milaRankLastWeek =
    //         _historicGameWeeks
    //             .Where(h => h.User.Id != User.Id && h.Event.GameWeek == Event.GameWeek - 1)
    //             .Sum(o =>
    //                 o.MilaScores.CumulativeTotalMilaScore
    //                 > _historicGameWeeks
    //                     .Where(h => h.User.Id == User.Id)
    //                     .FirstOrDefault(h => h.Event.GameWeek == Event.GameWeek - 1)
    //                     ?.MilaScores.CumulativeTotalMilaScore
    //                     ? 1
    //                     : 0
    //             ) + 1;
    //     Position = new Position(gwPosition, milaRank, milaRankLastWeek);
    // }
    //
    // private void SetMilaScores(List<MilaRuleResult> results)
    // {
    //     var totalMilaScore = results.Sum(r => r.Points);
    //     var totalCumulativeAvgMilaScore = Math.Round(
    //         MilaScores.CumulativeTotalMilaScore / Event.GameWeek,
    //         2
    //     );
    //     var cumulativeTotalMilaScore =
    //         _historicGameWeeks
    //             .Where(u => u.User.Id == User.Id)
    //             .Sum(h => h.MilaScores.TotalMilaScore) + MilaScores.TotalMilaScore;
    //     var avgCumulativeTotalMilaScore = Math.Round(
    //         (
    //             _historicGameWeeks.Sum(h => h.MilaScores.TotalMilaScore)
    //             + Opponents.Sum(h => h.MilaScores.TotalMilaScore)
    //             + MilaScores.TotalMilaScore
    //         ) / (_historicGameWeeks.Count + Opponents.Count + 1),
    //         2
    //     );
    //
    //     MilaScores = new MilaScores(
    //         totalMilaScore,
    //         totalCumulativeAvgMilaScore,
    //         cumulativeTotalMilaScore,
    //         avgCumulativeTotalMilaScore
    //     );
    // }
};
