using Milabowl.Domain.Entities.Fantasy;
using Milabowl.Domain.Import.FantasyDTOs;

namespace Milabowl.Domain.Import;

public interface IDataImportBusiness
{
    Task<IList<Event>> ImportEvents(BootstrapRootDTO bootstrapBootstrapRoot, IList<Event> eventsFromDb);
    Task<IList<Fixture>> ImportFixtures(IList<FixtureDTO> fixtureDtos, IList<Fixture> fixturesFromDb, IList<Event> events, IList<Team> teams);
    Task<IList<Team>> ImportTeams(BootstrapRootDTO bootstrapBootstrapRoot, IList<Team> teamsFromDb);
    Task<IList<Player>> ImportPlayers(BootstrapRootDTO bootstrapBootstrapRoot, IList<Team> teams, IList<Player> playersFromDb);
    Task<League> ImportLeague(LeagueRootDTO leagueRoot, IList<League> leaguesFromDb);
    Task<IList<User>> ImportUsers(LeagueRootDTO leagueRoot, IList<User> usersFromDb);
    Task<IList<UserLeague>> ImportUserLeagues(IList<User> users, League league, IList<UserLeague> userLeaguesFromDb);
    Task<IList<PlayerEvent>> ImportPlayerEvents(
        EventRootDTO eventRootDto, 
        Event finishedEvent, 
        IList<Player> players,
        IList<PlayerEvent> playerEventsFromDb,
        IList<ElementHistoryRootDTO> historyRootDtos,
        IList<FixtureDTO> fixtures
    );
    Task<IList<UserHeadToHeadEvent>> ImportUserHeadToHeadEvents(
        HeadToHeadEventRootDTO headToHeadEventDto,
        Event finishedEvent,
        IList<User> users,
        IList<UserHeadToHeadEvent> playerHeadToHeadEventsFromDb
    );
    Task<Lineup> ImportLineup(PicksRootDTO picksRootDto, Event finishedEvent, User user, IList<Lineup> lineupsFromDb);
    Task<IList<PlayerEventLineup>> ImportPlayerEventLineup(PicksRootDTO picksRoot, Event finishedEvent, Lineup lineup, IList<PlayerEvent> playerEvents, IList<PlayerEventLineup> playerEventLineupsFromDb);
}

public class DataImportBusiness: IDataImportBusiness
{
    private readonly IFantasyMapper _fantasyMapper;
    private readonly IImportRepository _repository;

    public DataImportBusiness(IFantasyMapper fantasyMapper, IImportRepository repository)
    {
        this._fantasyMapper = fantasyMapper;
        _repository = repository;
    }

    public async Task<IList<Event>> ImportEvents(BootstrapRootDTO bootstrapBootstrapRoot, IList<Event> eventsFromDb)
    {
        var events = bootstrapBootstrapRoot.Events.Select(this._fantasyMapper.GetEventFromEventDTO).ToList();

        foreach (var evt in events)
        {
            var eventFromDb = eventsFromDb.FirstOrDefault(e => e.FantasyEventId == evt.FantasyEventId);
            if (eventFromDb == null)
            {
                await _repository.AddAsync(evt);
            }
            else
            {
                evt.EventId = eventFromDb.EventId;
                _repository.Update(evt);
            }
        }

        return events;
    }

    public async Task<IList<Fixture>> ImportFixtures(IList<FixtureDTO> fixtureDtos, IList<Fixture> fixturesFromDb, IList<Event> events, IList<Team> teams)
    {
        var fixtures = new List<Fixture>();

        foreach (var fixtureDto in fixtureDtos)
        {
            var evt = events.FirstOrDefault(e => e.FantasyEventId == fixtureDto.@event);
            var homeTeam = teams.FirstOrDefault(t => t.FantasyTeamId == fixtureDto.team_h);
            var awayTeam = teams.FirstOrDefault(t => t.FantasyTeamId == fixtureDto.team_a);
            var fixture = this._fantasyMapper.GetFixtureFromFixtureDTO(fixtureDto, evt, homeTeam, awayTeam);

            var fixtureFromDb = fixturesFromDb.FirstOrDefault(f => f.FantasyFixtureId == fixture.FantasyFixtureId);
            if (fixtureFromDb == null)
            {
                await _repository.AddAsync(fixture);
                fixtures.Add(fixture);
            }
            else
            {
                fixture.FixtureId = fixtureFromDb.FixtureId;
                _repository.Update(fixture);
                fixtures.Add(fixture);
            }
        }

        return fixtures;
    }

    public async Task<IList<Team>> ImportTeams(BootstrapRootDTO bootstrapBootstrapRoot, IList<Team> teamsFromDb)
    {
        var teams = bootstrapBootstrapRoot.Teams.Select(this._fantasyMapper.GetTeamFromTeamDTO).ToList();

        foreach (var team in teams)
        {
            var teamFromDb = teamsFromDb.FirstOrDefault(t => t.FantasyTeamCode == team.FantasyTeamCode);

            if (teamFromDb == null)
            {
                await _repository.AddAsync(team);
            }
            else
            {
                team.TeamId = teamFromDb.TeamId;
                _repository.Update(team);
            }
        }

        return teams;
    }

    public async Task<IList<Player>> ImportPlayers(BootstrapRootDTO bootstrapBootstrapRoot, IList<Team> teams, IList<Player> playersFromDb)
    {
        var players = bootstrapBootstrapRoot.Players.Select(p =>
            this._fantasyMapper.GetPlayerFromPlayerDTO(p, teams)
        ).ToList();

        foreach (var player in players)
        {
            var playerFromDb = playersFromDb.FirstOrDefault(p => p.FantasyPlayerId == player.FantasyPlayerId);

            if (playerFromDb == null)
            {
                await _repository.AddAsync(player);
            }
            else
            {
                player.PlayerId = playerFromDb.PlayerId;
                _repository.Update(player);
            }
        }

        return players;
    }

    public async Task<League> ImportLeague(LeagueRootDTO leagueRoot, IList<League> leaguesFromDb)
    {
        var leagueDto = leagueRoot.league;

        var leagueFromDb = leaguesFromDb.FirstOrDefault(l => l.FantasyLeagueId == leagueDto.id);

        var league = this._fantasyMapper.GetLeagueFromLeagueDTO(leagueDto);

        if (leagueFromDb == null)
        {
            await _repository.AddAsync(league);
            return league;
        }

        league.LeagueId = leagueFromDb.LeagueId;
        _repository.Update(league);

        return league;
    }

    public async Task<IList<User>> ImportUsers(LeagueRootDTO leagueRoot, IList<User> usersFromDb)
    {
        var users = leagueRoot.standings.results.Select(this._fantasyMapper.GetUserFromResultDTO).ToList();

        foreach (var user in users)
        {
            var userFromDb = usersFromDb.FirstOrDefault(u => u.FantasyEntryId == user.FantasyEntryId);
            if (userFromDb == null)
            {
                await _repository.AddAsync(user);
            }
            else
            {
                user.UserId = userFromDb.UserId;
                _repository.Update(user);
            }
        }

        return users;
    }

    public async Task<IList<UserLeague>> ImportUserLeagues(IList<User> users, League league, IList<UserLeague> userLeaguesFromDb)
    {
        var userLeagues = users.Select(u =>
            this._fantasyMapper.GetUserLeagueFromUserAndLeague(u, league)
        ).ToList();

        foreach (var userLeague in userLeagues)
        {
            var userLeagueFromDb = userLeaguesFromDb.FirstOrDefault(u =>
                u.League.FantasyLeagueId == userLeague.League.FantasyLeagueId && u.User.FantasyEntryId == userLeague.User.FantasyEntryId);

            if (userLeagueFromDb == null)
            {
                await _repository.AddAsync(userLeague);
            }
            else
            {
                userLeague.UserLeagueId = userLeagueFromDb.UserLeagueId;
                _repository.Update(userLeague);
            }
        }

        return userLeagues;
    }

    public async Task<IList<PlayerEvent>> ImportPlayerEvents(
        EventRootDTO eventRootDto, 
        Event finishedEvent, 
        IList<Player> players, 
        IList<PlayerEvent> playerEventsFromDb,
        IList<ElementHistoryRootDTO> historyRootDtos,
        IList<FixtureDTO> fixtures)
    {
        var playerEventsFromDbForEvent = playerEventsFromDb
            .Where(pe => pe.Event.FantasyEventId == finishedEvent.FantasyEventId)
            .ToList();

        var playerEvents = eventRootDto.elements.Select(e =>
            this._fantasyMapper.GetPlayerEvent(e, finishedEvent, players, historyRootDtos, fixtures)
        ).ToList();

        foreach (var playerEvent in playerEvents)
        {
            var playerEventFromDb = playerEventsFromDbForEvent.FirstOrDefault(
                pe => pe.Event.FantasyEventId == playerEvent.Event.FantasyEventId
                      && pe.Player.FantasyPlayerId == playerEvent.Player.FantasyPlayerId
            );

            if (playerEventFromDb == null)
            {
                await _repository.AddAsync(playerEvent);
            }
            else
            {
                playerEvent.PlayerEventId = playerEventFromDb.PlayerEventId;
                _repository.Update(playerEvent);
            }
        }

        return playerEvents;
    }

    public async Task<IList<UserHeadToHeadEvent>> ImportUserHeadToHeadEvents(
            
        HeadToHeadEventRootDTO headToHeadEventDto,
        Event finishedEvent,
        IList<User> users,
        IList<UserHeadToHeadEvent> playerHeadToHeadEventsFromDb
    )
    {
        var playerHeadToHeadEventsFromDbForEvent = playerHeadToHeadEventsFromDb
            .Where(pe => pe.Event.FantasyEventId == finishedEvent.FantasyEventId)
            .ToList();

        var playerHeadToHeadEvents = headToHeadEventDto.results.SelectMany(r =>
            this._fantasyMapper.GetUserHeadToHeadEvents(r, finishedEvent, users)
        ).ToList();

        foreach (var playerHeadToHeadEvent in playerHeadToHeadEvents)
        {
            var playerEventFromDb = playerHeadToHeadEventsFromDbForEvent.FirstOrDefault(
                pe => pe.Event.FantasyEventId == playerHeadToHeadEvent.Event.FantasyEventId
                      && pe.User.FantasyEntryId == playerHeadToHeadEvent.User.FantasyEntryId
            );

            if (playerEventFromDb == null)
            {
                await _repository.AddAsync(playerHeadToHeadEvent);
            }
            else
            {
                playerHeadToHeadEvent.UserHeadToHeadEventID = playerEventFromDb.UserHeadToHeadEventID;
                _repository.Update(playerHeadToHeadEvent);
            }
        }

        return playerHeadToHeadEvents;
    }

    public async Task<Lineup> ImportLineup(PicksRootDTO picksRootDto, Event finishedEvent, User user, IList<Lineup> lineupsFromDb)
    {
        var lineupFromDb = lineupsFromDb.FirstOrDefault(l => l.FkEventId == finishedEvent.EventId && l.FkUserId == user.UserId);

        var lineup = this._fantasyMapper.GetLineup(picksRootDto, finishedEvent, user);

        if (lineupFromDb == null)
        {
            await _repository.AddAsync(lineup);
        }
        else
        {
            lineup.LineupId = lineupFromDb.LineupId;
            _repository.Update(lineup);
        }

        return lineup;
    }

    public async Task<IList<PlayerEventLineup>> ImportPlayerEventLineup(PicksRootDTO picksRoot, Event finishedEvent, Lineup lineup, IList<PlayerEvent> playerEvents, IList<PlayerEventLineup> playerEventLineupsFromDb)
    {
        var playerEventLineups = picksRoot.picks.Select(p => 
            this._fantasyMapper.GetPlayerEventLineup(p, lineup, playerEvents, finishedEvent)
        ).ToList();
        
        foreach (var playerEventLineup in playerEventLineups)
        {
            var playerLineupFromDb = playerEventLineupsFromDb.FirstOrDefault(pel =>
                pel.PlayerEvent.FantasyPlayerEventId == playerEventLineup.PlayerEvent.FantasyPlayerEventId &&
                pel.Lineup.Event.FantasyEventId == playerEventLineup.Lineup.Event.FantasyEventId
                && pel.Lineup.User.FantasyEntryId == playerEventLineup.Lineup.User.FantasyEntryId);
                
            if (playerLineupFromDb == null)
            {
                await _repository.AddAsync(playerEventLineup);
            }
            else
            {
                playerEventLineup.PlayerEventLineupId = playerLineupFromDb.PlayerEventLineupId;
                _repository.Update(playerEventLineup);
            }
        }

        return playerEventLineups;
    }
}