using System.Collections.ObjectModel;
using Milabowl.Processing.DataImport.FplDtos;

namespace Milabowl.Processing.DataImport.Models;

public record ManagerGameWeekState
{
    public required User User { get; init; }
    public required Event Event { get; init; }
    public required IReadOnlyList<PlayerEvent> Lineup { get; init; }
    public required IReadOnlyList<Transfer> TransfersIn { get; init; }
    public required IReadOnlyList<Transfer> TransfersOut { get; init; }
    public required int TransferCost { get; init; }
    public required HeadToHead HeadToHead { get; init; }
    public ManagerGameWeekState? PreviousGameWeek { get; init; }
    public required IList<ManagerGameWeekState> History { get; init; }
    public required decimal TotalScore { get; init; }
    public string? ActiveChip { get; init; }
    public required IReadOnlyList<ManagerGameWeekState> Opponents { get; init; }
    public required AutoSub AutoSubs { get; init; }
}

public record Transfer
{
    public required int TotalPoints { get; init; }
    public required string FirstName { get; init; }
    public required string Surname { get; init; }
    public required int FantasyPlayerEventId { get; init; }
};

public record AutoSub
{
    public required IList<Sub> In { get; init; }
    public required IList<Sub> Out { get; init; }
}

public record Sub
{
    public required int TotalPoints { get; init; }
    public required string FirstName { get; init; }
    public required string Surname { get; init; }
    public required int FantasyPlayerEventId { get; init; }
}

public static class StateFactory
{
    public static ManagerGameWeekState CreateUserState(
        Event @event,
        HeadToHead headToHead,
        User user,
        IList<PlayerEvent> lineup,
        AutoSub autoSubs,
        string? activeChip,
        int transferCost,
        IList<ManagerGameWeekState> historicGameWeeks,
        EventRootDto eventRootDto,
        IList<ManagerGameWeekState> opponents
    )
    {
        historicGameWeeks = historicGameWeeks
            .Where(e => e.Event.GameWeek < @event.GameWeek)
            .ToList();

        var previousGameWeek = historicGameWeeks.FirstOrDefault(h =>
            h.Event.GameWeek == @event.GameWeek - 1 && h.User.Id == user.Id
        );

        return new ManagerGameWeekState
        {
            PreviousGameWeek = previousGameWeek,
            HeadToHead = headToHead,
            ActiveChip = activeChip,
            User = user,
            Event = @event,
            AutoSubs = autoSubs,
            Lineup = lineup.AsReadOnly(),
            TotalScore = lineup.Sum(l => l.TotalPoints * l.Multiplier),
            TransferCost = transferCost,
            TransfersIn = previousGameWeek is null
                ? new ReadOnlyCollection<Transfer>([])
                : lineup
                    .Where(pe =>
                        previousGameWeek.Lineup.All(ipe =>
                            ipe.FantasyPlayerEventId != pe.FantasyPlayerEventId
                        )
                    )
                    .Select(s => new Transfer
                    {
                        TotalPoints = s.TotalPoints,
                        FirstName = s.FirstName,
                        Surname = s.Surname,
                        FantasyPlayerEventId = s.FantasyPlayerEventId,
                    })
                    .ToList()
                    .AsReadOnly(),
            TransfersOut = previousGameWeek is null
                ? new ReadOnlyCollection<Transfer>([])
                : previousGameWeek
                    .Lineup.Where(pe =>
                        lineup.All(ipe => ipe.FantasyPlayerEventId != pe.FantasyPlayerEventId)
                    )
                    .Select(s => new Transfer
                    {
                        TotalPoints = eventRootDto
                            .Elements.First(e => e.Id == s.FantasyPlayerEventId)
                            .Stats.TotalPoints,
                        FirstName = s.FirstName,
                        Surname = s.Surname,
                        FantasyPlayerEventId = s.FantasyPlayerEventId,
                    })
                    .ToList()
                    .AsReadOnly(),
            History = historicGameWeeks.Where(h => h.User.Id == user.Id).ToList().AsReadOnly(),
            Opponents = opponents.AsReadOnly(),
        };
    }
}
