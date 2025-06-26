using System.Collections.ObjectModel;
using Milabowl.Processing.DataImport.FplDtos;

namespace Milabowl.Processing.DataImport.Models;

public record MilaGameWeekState
{
    public required UserState User { get; init; }
    public required IReadOnlyList<UserState> Opponents { get; init; }
}

public record Sub
{
    public required int TotalPoints { get; init; }
    public required string FirstName { get; init; }
    public required string Surname { get; init; }
    public required int FantasyPlayerEventId { get; init; }
};

public record UserState
{
    public required User User { get; init; }
    public required Event Event { get; init; }
    public required IReadOnlyList<PlayerEvent> Lineup { get; init; }
    public required IReadOnlyList<Sub> SubsIn { get; init; }
    public required IReadOnlyList<Sub> SubsOut { get; init; }
    public required HeadToHead HeadToHead { get; init; }
    public UserState? PreviousGameWeek { get; init; }
    public required IList<UserState> History { get; init; }
    public required decimal TotalScore { get; init; }
    public string? ActiveChip { get; init; }
};

public static class StateFactory
{
    public static MilaGameWeekState CreateMilaGameWeekState(UserState user, IList<UserState> opponents)
    {
        return new MilaGameWeekState
        {
            User = user,
            Opponents = opponents.Where(u => u.User.Id != user.User.Id).ToList().AsReadOnly()
        };
    }

    public static UserState CreateUserState(
        Event @event,
        HeadToHead headToHead,
        User user,
        IList<PlayerEvent> lineup,
        string? activeChip,
        IList<UserState> historicGameWeeks,
        EventRootDTO eventRootDto
    )
    {
        historicGameWeeks = historicGameWeeks
            .Where(e => e.Event.GameWeek < @event.GameWeek)
            .ToList();

        var previousGameWeek = historicGameWeeks
            .FirstOrDefault(h =>
                h.Event.GameWeek == @event.GameWeek - 1 && h.User.Id == user.Id
            );

        return new UserState
        {
            PreviousGameWeek = previousGameWeek,
            HeadToHead = headToHead,
            ActiveChip = activeChip,
            User = user,
            Event = @event,
            Lineup = lineup.AsReadOnly(),
            TotalScore = lineup.Sum(l => l.TotalPoints * l.Multiplier),
            SubsIn = previousGameWeek is null
                ? new ReadOnlyCollection<Sub>([
                ])
                : lineup
                    .Where(pe =>
                        previousGameWeek.Lineup.All(ipe =>
                            ipe.FantasyPlayerEventId != pe.FantasyPlayerEventId
                        )
                    )
                    .Select(s => new Sub{
                        TotalPoints = s.TotalPoints,
                        FirstName = s.FirstName,
                        Surname = s.Surname,
                        FantasyPlayerEventId = s.FantasyPlayerEventId
                    })
                    .ToList()
                    .AsReadOnly(),
            SubsOut = previousGameWeek is null
                ? new ReadOnlyCollection<Sub>([
                ])
                : previousGameWeek
                    .Lineup.Where(pe =>
                        lineup.All(ipe => ipe.FantasyPlayerEventId != pe.FantasyPlayerEventId)
                    )
                    .Select(s => new Sub
                    {
                        TotalPoints = eventRootDto
                            .elements.First(e => e.id == s.FantasyPlayerEventId)
                            .stats.total_points,
                        FirstName = s.FirstName,
                        Surname = s.Surname,
                        FantasyPlayerEventId = s.FantasyPlayerEventId
                    })
                    .ToList()
                    .AsReadOnly(),
            History = historicGameWeeks.Where(h => h.User.Id == user.Id)
                .ToList()
                .AsReadOnly(),
        };
    }
}
