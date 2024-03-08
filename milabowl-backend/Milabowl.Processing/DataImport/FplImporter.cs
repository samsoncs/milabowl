using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Milabowl.Processing.DataImport.Models;
using Milabowl.Processing.Processing;

namespace Milabowl.Processing.DataImport;

public class FplImporter
{
    private readonly IFplService _fplService;
    private readonly IRulesProcessor _rulesProcessor;

    public FplImporter(IFplService fplService, IRulesProcessor rulesProcessor)
    {
        _fplService = fplService;
        _rulesProcessor = rulesProcessor;
    }

    public async Task<ReadOnlyDictionary<int, ImmutableList<UserGameWeek>>> Import()
    {
        var bootstrapRoot = await _fplService.GetBootstrapRoot();
        var events = bootstrapRoot.Events;
        var players = bootstrapRoot.Players;
        var leagueRoot = await _fplService.GetLeagueRoot();
        var users = leagueRoot.standings.results;
        List<UserGameWeek> gameWeeks = [];
        foreach (var finishedEvent in events.Where(e => e is { Finished: true, DataChecked: true }))
        {
            var userGameWeeks = new List<UserGameWeek>();
            var eventRootDto = await _fplService.GetEventRoot(finishedEvent.Id);
            var headToHeadEventRootDto = await _fplService.GetHead2HeadEventRoot(finishedEvent.Id);
            foreach (var user in users)
            {
                var picksRoot = await _fplService.GetPicksRoot(finishedEvent.Id, user.entry);
                var historicGameWeeks = new List<UserGameWeek>(gameWeeks);
                var userGameWeek = new UserGameWeek(
                    finishedEvent.ToEvent(),
                    headToHeadEventRootDto.ToHeadToHeadEvent(user.entry),
                    user.ToUser(),
                    picksRoot.ToLineup(eventRootDto, players),
                    picksRoot.active_chip,
                    historicGameWeeks
                );

                userGameWeeks.Add(userGameWeek);
            }

            foreach (var userGameWeek in userGameWeeks)
            {
                userGameWeek.AddOpponentsForGameWeek(userGameWeeks);
                var rulesResults = _rulesProcessor.CalculateForUserGameWeek(userGameWeek);
                userGameWeek.AddMilaRuleResults(rulesResults);
            }

            gameWeeks.AddRange(userGameWeeks);
        }

        return gameWeeks
            .GroupBy(g => g.Event.GameWeek)
            .ToDictionary(g => g.Key, g => g.ToImmutableList())
            .AsReadOnly();
    }
}
