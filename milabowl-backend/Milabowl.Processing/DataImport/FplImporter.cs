using System.Collections.ObjectModel;

namespace Milabowl.Processing.DataImport;

public class FplImporter
{
    private readonly IFplService _fplService;

    public FplImporter(IFplService fplService)
    {
        _fplService = fplService;
    }

    public async Task<ReadOnlyDictionary<int, List<UserGameWeek>>> Import()
    {
        var userGameWeeksByGameWeek = new Dictionary<int, List<UserGameWeek>>();

        var bootstrapRoot = await _fplService.GetBootstrapRoot();
        var events = bootstrapRoot.Events;//.Select(e => _mapper.GetEventFromEventDTO(e));
        // var teams = bootstrapRoot.Teams.Select(t => _mapper.GetTeamFromTeamDTO(t)).ToList();
        var players = bootstrapRoot.Players;//.Select(p => _mapper.GetPlayerFromPlayerDTO(p, teams)).ToList();
        // var fixtures = (await _fplService.GetFixtures()).Where(e => e.@event != null)
        //     .Select(f =>
        //     {
        //         var evt = events.First(e => e.FantasyEventId == f.@event);
        //         var homeTeam = teams.First(t => t.FantasyTeamId == f.team_h);
        //         var awayTeam = teams.First(t => t.FantasyTeamId == f.team_a);
        //
        //         return _mapper.GetFixtureFromFixtureDTO(f, evt, homeTeam, awayTeam);
        //     })
        //     .ToList();

        // var playerHistoryRoots = players.Select(async p => await _fplService.GetPlayerHistoryRoot(p.FantasyPlayerId))
        //     .Select(t => t.Result)
        //     .Where(t => t != null)
        //     .ToList();

        var leagueRoot = await _fplService.GetLeagueRoot();
        // var league = _mapper.GetLeagueFromLeagueDTO(leagueRoot.league);
        var users = leagueRoot.standings.results;
            // .Select(r => _mapper.GetUserFromResultDTO(r))
            // .ToList();

        // User history => Not needed for Milabowl, but perhaps stats.
        // foreach (var user in users)
        // {
        //     var entryRoot = await _fplService.GetEntryRoot(user.FantasyEntryId);
        //     var userHistories = entryRoot.Past
        //         .Select(e => _mapper.GetUserHistory(e, user));
        // }



        foreach (var finishedEvent in events.Where(e => e.Finished && e.DataChecked))
        {
            var userGameWeeks = new List<UserGameWeek>();
            var eventRootDto = await _fplService.GetEventRoot(finishedEvent.Id);
            var headToHeadEventRootDto = await _fplService.GetHead2HeadEventRoot(finishedEvent.Id);
            // var headToHead = _mapper.GetUserHeadToHeadEvents(headToHeadEventRootDto.results, finishedEvent, users);

            foreach (var user in users)
            {
                var picksRoot = await _fplService.GetPicksRoot(finishedEvent.Id, user.entry);
                if (picksRoot.picks is null)
                {
                    continue;
                }

                // var lineup = _mapper.GetLineup(picksRoot, finishedEvent, user);
                //var playerEventLineup = picksRoot.picks
                //   .Select(p => _mapper.GetPlayerEventLineup(p, lineup, playerEvents, finishedEvent));

                var lineup = picksRoot.picks.Select(p =>
                {
                    var element = eventRootDto.elements.First(e => e.id == p.element);
                    var player = players.First(plr => plr.Id == element.id);

                    return new PlayerEvent(
                        player.FirstName,
                        player.SecondName,
                        element.id,
                        element.stats.minutes,
                        element.stats.goals_scored,
                        element.stats.assists,
                        element.stats.goals_conceded,
                        element.stats.clean_sheets,
                        element.stats.penalties_saved,
                        element.stats.own_goals,
                        element.stats.penalties_missed,
                        element.stats.yellow_cards,
                        element.stats.red_cards,
                        element.stats.saves,
                        element.stats.bonus,
                        element.stats.bps,
                        element.stats.influence,
                        element.stats.creativity,
                        element.stats.threat,
                        element.stats.ict_index,
                        element.stats.total_points,
                        element.stats.in_dreamteam,
                        p.multiplier,
                        p.is_captain,
                        p.is_vice_captain,
                        p.position
                    );
                }).ToList();

                // Need to map based on if entry_1 or entry_2 match
                // var headToHeadForUser = headToHeadEventRootDto.results
                //     .First(r => r.entry_1_entry == user.FantasyEntryId
                //                 || r.entry_2_entry == user.FantasyEntryId);


                userGameWeeks.Add(
                    new UserGameWeek(
                            new Event(finishedEvent.Id, finishedEvent.Name),
                            new HeadToHeadEvent(
                                1,1,1,1,1,false, 1, false
                                ),
                            new User(
                                user.id,
                                user.player_name,
                                user.entry_name,
                                user.rank,
                                user.last_rank,
                                user.event_total
                            ),
                            lineup,
                            picksRoot.active_chip
                        )
                );

            }

            foreach (var userGameWeek in userGameWeeks)
            {
                userGameWeek.AddOpponentsForGameWeek(userGameWeeks);
            }

            userGameWeeksByGameWeek.Add(finishedEvent.Id, userGameWeeks);
        }

        return userGameWeeksByGameWeek.AsReadOnly();
    }
}
