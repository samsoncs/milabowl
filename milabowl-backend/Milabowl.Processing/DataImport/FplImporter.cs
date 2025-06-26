using Milabowl.Processing.DataImport.MilaDtos;
using Milabowl.Processing.DataImport.Models;
using Milabowl.Processing.Processing;

namespace Milabowl.Processing.DataImport;

public class FplImporter
{
    private readonly IFplService _fplService;

    public FplImporter(IFplService fplService)
    {
        _fplService = fplService;
    }

    public async Task<IReadOnlyList<MilaGameWeekState>> ImportFplDataForRulesProcessing()
    {
        var bootstrapRoot = await _fplService.GetBootstrapRoot();
        var events = bootstrapRoot.Events;
        var players = bootstrapRoot.Players;
        var teams = bootstrapRoot.Teams;
        var leagueRoot = await _fplService.GetLeagueRoot();
        var users = leagueRoot.standings.results;
        List<UserState> userStates = [];
        foreach (var finishedEvent in events.Where(e => e is { Finished: true, DataChecked: true }))
        {
            var eventRootDto = await _fplService.GetEventRoot(finishedEvent.Id);
            var headToHeadEventRootDto = await _fplService.GetHead2HeadEventRoot(finishedEvent.Id);
            foreach (var user in users)
            {
                var picksRoot = await _fplService.GetPicksRoot(finishedEvent.Id, user.entry);
                var historicGameWeeks = new List<UserState>(
                    userStates.Where(u => u.Event.GameWeek < finishedEvent.Id)
                );

                var userGameWeek = StateFactory.CreateUserState(
                    finishedEvent.ToEvent(),
                    headToHeadEventRootDto.ToHeadToHeadEvent(user.entry),
                    user.ToUser(),
                    picksRoot.ToLineup(eventRootDto, players, teams),
                    picksRoot.active_chip,
                    historicGameWeeks,
                    eventRootDto
                );

                userStates.Add(userGameWeek);
            }
        }

        return userStates
            .GroupBy(u => u.Event)
            .SelectMany(s =>
                s.ToList()
                    .Select(u => StateFactory.CreateMilaGameWeekState(
                        u,
                        s.ToList().Where(x => x.User.Id != u.User.Id).ToList()
                    ))
            )
            .ToList();
    }

    public async Task<FplResults> ImportFplData()
    {
        var bootstrapRoot = await _fplService.GetBootstrapRoot();
        var events = bootstrapRoot.Events;
        var players = bootstrapRoot.Players;
        var teams = bootstrapRoot.Teams;
        var leagueRoot = await _fplService.GetLeagueRoot();
        var users = leagueRoot.standings.results;

        var fplUserGameWeekResult = new List<FplUserGameWeekResult>();

        foreach (var finishedEvent in events.Where(e => e is { Finished: true, DataChecked: true }))
        {
            var eventRootDto = await _fplService.GetEventRoot(finishedEvent.Id);

            foreach (var user in users)
            {
                var picksRoot = await _fplService.GetPicksRoot(finishedEvent.Id, user.entry);

                var lineup = picksRoot.ToLineup(eventRootDto, players, teams);

                fplUserGameWeekResult.Add(new FplUserGameWeekResult(
                    finishedEvent.Id,
                    user.entry_name,
                    user.total,
                    lineup.Select(l => new FplPlayerEventResult(
                        l.WebName,
                        l.TeamName,
                        l.TotalPoints,
                        l.PlayerPositionString,
                        l.IsCaptain,
                        l.IsViceCaptain,
                        l.Multiplier == 0)
                    ).ToList()
                ));

            }
        }

        return new FplResults(fplUserGameWeekResult);
    }
}
