using System.Collections.ObjectModel;
using Milabowl.Processing.DataImport.FplDtos;

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

public record Sub(int TotalPoints, string FirstName, string Surname);

public class UserState
{
    public User User { get; }
    public Event Event { get; }
    public IReadOnlyList<PlayerEvent> Lineup { get; }
    public IReadOnlyList<Sub> SubsIn { get; }
    public IReadOnlyList<Sub> SubsOut { get; }
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
        IList<UserState> historicGameWeeks,
        EventRootDTO eventRootDto
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
            ? new ReadOnlyCollection<Sub>([])
            : Lineup
                .Where(pe =>
                    PreviousGameWeek.Lineup.All(ipe =>
                        ipe.FantasyPlayerEventId != pe.FantasyPlayerEventId
                    )
                )
                .Select(s => new Sub(s.TotalPoints, s.FirstName, s.Surname))
                .ToList()
                .AsReadOnly();
        SubsOut = PreviousGameWeek is null
            ? new ReadOnlyCollection<Sub>([])
            : PreviousGameWeek
                .Lineup.Where(pe =>
                    Lineup.All(ipe => ipe.FantasyPlayerEventId != pe.FantasyPlayerEventId)
                )
                .Select(s => new Sub(
                    eventRootDto
                        .elements.First(e => e.id == s.FantasyPlayerEventId)
                        .stats.total_points,
                    s.FirstName,
                    s.Surname
                ))
                .ToList()
                .AsReadOnly();
        History = historicGameWeeks.Where(h => h.User.Id == User.Id).ToList().AsReadOnly();
        ActiveChip = activeChip;
        TotalScore = lineup.Sum(l => l.TotalPoints * l.Multiplier);
    }
};
