using Milabowl.Processing.DataImport.MilaDtos;
using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.DataImport;

public class FplImporter
{
    private readonly IFplService _fplService;

    public FplImporter(IFplService fplService)
    {
        _fplService = fplService;
    }

    public async Task<ImportData> ImportFplDataForRulesProcessing()
    {
        var bootstrapRoot = await _fplService.GetBootstrapRoot();
        var events = bootstrapRoot.Events;
        var players = bootstrapRoot.Players;
        var teams = bootstrapRoot.Teams;
        var leagueRoot = await _fplService.GetLeagueRoot();
        var users = leagueRoot.Standings.Results;
        List<ManagerGameWeekState> userStates = [];
        foreach (var @event in events.Where(e => e.DeadlineTime < DateTime.UtcNow))
        {
            var eventRootDto = await _fplService.GetEventRoot(@event.Id);
            var headToHeadEventRootDto = await _fplService.GetHead2HeadEventRoot(@event.Id);
            foreach (var user in users)
            {
                var picksRoot = await _fplService.GetPicksRoot(@event.Id, user.Entry);
                var historicGameWeeks = new List<ManagerGameWeekState>(
                    userStates.Where(u => u.Event.GameWeek < @event.Id)
                );

                var userGameWeek = StateFactory.CreateUserState(
                    @event.ToEvent(),
                    headToHeadEventRootDto.ToHeadToHeadEvent(user.Entry),
                    user.ToUser(),
                    picksRoot.ToLineup(eventRootDto, players, teams),
                    picksRoot.ToAutoSubs(eventRootDto, players),
                    picksRoot.ActiveChip,
                    picksRoot.EntryHistory.EventTransfersCost,
                    historicGameWeeks,
                    eventRootDto,
                    new List<ManagerGameWeekState>()
                );

                userStates.Add(userGameWeek);
            }
        }

        var lastEvent = events.Last(e => e.DeadlineTime < DateTime.UtcNow);

        return new ImportData
        {
            ManagerGameWeekStates = userStates
                .GroupBy(u => u.Event)
                .SelectMany(s =>
                    s.ToList()
                        .Select(u =>
                            u with
                            {
                                Opponents = s.ToList()
                                    .Where(x => x.User.Id != u.User.Id)
                                    .ToList()
                                    .AsReadOnly(),
                            }
                        )
                )
                .ToList(),
            IsLive = lastEvent is { Finished: false, DataChecked: false },
        };
    }

    public async Task<FplResults> ImportFplData()
    {
        var bootstrapRoot = await _fplService.GetBootstrapRoot();
        var events = bootstrapRoot.Events;
        var players = bootstrapRoot.Players;
        var teams = bootstrapRoot.Teams;
        var leagueRoot = await _fplService.GetLeagueRoot();
        var users = leagueRoot.Standings.Results;

        var fplUserGameWeekResult = new List<FplUserGameWeekResult>();

        foreach (var finishedEvent in events.Where(e => e is { Finished: true, DataChecked: true }))
        {
            var eventRootDto = await _fplService.GetEventRoot(finishedEvent.Id);

            foreach (var user in users)
            {
                var picksRoot = await _fplService.GetPicksRoot(finishedEvent.Id, user.Entry);

                var lineup = picksRoot.ToLineup(eventRootDto, players, teams);

                fplUserGameWeekResult.Add(
                    new FplUserGameWeekResult(
                        finishedEvent.Id,
                        user.EntryName,
                        user.Total,
                        lineup
                            .Select(l => new FplPlayerEventResult(
                                l.WebName,
                                l.TeamName,
                                l.TotalPoints,
                                l.PlayerPositionString,
                                l.IsCaptain,
                                l.IsViceCaptain,
                                l.Multiplier == 0
                            ))
                            .ToList()
                    )
                );
            }
        }

        return new FplResults(fplUserGameWeekResult);
    }
}
