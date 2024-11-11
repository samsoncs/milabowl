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

    public async Task<IReadOnlyList<MilaGameWeekState>> Import()
    {
        var bootstrapRoot = await _fplService.GetBootstrapRoot();
        var events = bootstrapRoot.Events;
        var players = bootstrapRoot.Players;
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

                var userGameWeek = new UserState(
                    finishedEvent.ToEvent(),
                    headToHeadEventRootDto.ToHeadToHeadEvent(user.entry),
                    user.ToUser(),
                    picksRoot.ToLineup(eventRootDto, players),
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
                    .Select(u => new MilaGameWeekState(
                        u,
                        s.ToList().Where(x => x.User.Id != u.User.Id).ToList()
                    ))
            )
            .ToList();
    }
}
