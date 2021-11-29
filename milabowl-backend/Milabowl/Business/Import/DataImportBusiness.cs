using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Milabowl.Business.DTOs.Import;
using Milabowl.Business.Mappers;
using Milabowl.Infrastructure.Contexts;
using Milabowl.Infrastructure.Models;

namespace Milabowl.Business.Import
{
    public interface IDataImportBusiness
    {
        Task<IList<Event>> ImportEvents(FantasyContext db, BootstrapRootDTO bootstrapBootstrapRoot, IList<Event> eventsFromDb);
        Task<IList<Team>> ImportTeams(FantasyContext db, BootstrapRootDTO bootstrapBootstrapRoot, IList<Team> teamsFromDb);
        Task<IList<Player>> ImportPlayers(FantasyContext db, BootstrapRootDTO bootstrapBootstrapRoot, IList<Team> teams, IList<Player> playersFromDb);
        Task<League> ImportLeague(FantasyContext db, LeagueRootDTO leagueRoot, IList<League> leaguesFromDb);
        Task<IList<User>> ImportUsers(FantasyContext db, LeagueRootDTO leagueRoot, IList<User> usersFromDb);
        Task<IList<UserLeague>> ImportUserLeagues(FantasyContext db, IList<User> users, League league, IList<UserLeague> userLeaguesFromDb);
        Task<IList<PlayerEvent>> ImportPlayerEvents(FantasyContext db, EventRootDTO eventRootDto, Event finishedEvent, IList<Player> players, IList<PlayerEvent> playerEventsFromDb);
        Task<IList<PlayerHeadToHeadEvent>> ImportHeadToHeadPlayerEvents(FantasyContext db, HeadToHeadEventRootDTO headToHeadEventRootDto, Event finishedEvent, IList<Player> players, IList<PlayerHeadToHeadEvent> playerEventsFromDb);
        Task<Lineup> ImportLineup(FantasyContext db, Event finishedEvent, User user, IList<Lineup> lineupsFromDb);
        Task<IList<PlayerEventLineup>> ImportPlayerEventLineup(FantasyContext db, PicksRootDTO picksRoot, Event finishedEvent, Lineup lineup, IList<PlayerEvent> playerEvents, IList<PlayerEventLineup> playerEventLineupsFromDb);
    }

    public class DataImportBusiness: IDataImportBusiness
    {
        private readonly IFantasyMapper _fantasyMapper;

        public DataImportBusiness(IFantasyMapper fantasyMapper)
        {
            this._fantasyMapper = fantasyMapper;
        }

        public async Task<IList<Event>> ImportEvents(FantasyContext db, BootstrapRootDTO bootstrapBootstrapRoot, IList<Event> eventsFromDb)
        {
            var events = bootstrapBootstrapRoot.Events.Select(this._fantasyMapper.GetEventFromEventDTO).ToList();

            foreach (var evt in events)
            {
                var eventFromDb = eventsFromDb.FirstOrDefault(e => e.FantasyEventId == evt.FantasyEventId);
                if (eventFromDb == null)
                {
                    await db.Events.AddAsync(evt);
                }
                else
                {
                    evt.EventId = eventFromDb.EventId;
                    db.Events.Update(evt);
                }
            }

            return events;
        }

        public async Task<IList<Team>> ImportTeams(FantasyContext db, BootstrapRootDTO bootstrapBootstrapRoot, IList<Team> teamsFromDb)
        {
            var teams = bootstrapBootstrapRoot.Teams.Select(this._fantasyMapper.GetTeamFromTeamDTO).ToList();

            foreach (var team in teams)
            {
                var teamFromDb = teamsFromDb.FirstOrDefault(t => t.FantasyTeamCode == team.FantasyTeamCode);

                if (teamFromDb == null)
                {
                    await db.Teams.AddAsync(team);
                }
                else
                {
                    team.TeamId = teamFromDb.TeamId;
                    db.Teams.Update(team);
                }
            }

            return teams;
        }

        public async Task<IList<Player>> ImportPlayers(FantasyContext db, BootstrapRootDTO bootstrapBootstrapRoot, IList<Team> teams, IList<Player> playersFromDb)
        {
            var players = bootstrapBootstrapRoot.Players.Select(p =>
                this._fantasyMapper.GetPlayerFromPlayerDTO(p, teams)
            ).ToList();

            foreach (var player in players)
            {
                var playerFromDb = playersFromDb.FirstOrDefault(p => p.FantasyPlayerId == player.FantasyPlayerId);

                if (playerFromDb == null)
                {
                    await db.Players.AddAsync(player);
                }
                else
                {
                    player.PlayerId = playerFromDb.PlayerId;
                    db.Players.Update(player);
                }
            }

            return players;
        }

        public async Task<League> ImportLeague(FantasyContext db, LeagueRootDTO leagueRoot, IList<League> leaguesFromDb)
        {
            var leagueDto = leagueRoot.league;

            var leagueFromDb = leaguesFromDb.FirstOrDefault(l => l.FantasyLeagueId == leagueDto.id);

            var league = this._fantasyMapper.GetLeagueFromLeagueDTO(leagueDto);

            if (leagueFromDb == null)
            {
                await db.AddAsync(league);
                return league;
            }

            league.LeagueId = leagueFromDb.LeagueId;
            db.Leagues.Update(league);

            return league;
        }

        public async Task<IList<User>> ImportUsers(FantasyContext db, LeagueRootDTO leagueRoot, IList<User> usersFromDb)
        {
            var users = leagueRoot.standings.results.Select(this._fantasyMapper.GetUserFromResultDTO).ToList();

            foreach (var user in users)
            {
                var userFromDb = usersFromDb.FirstOrDefault(u => u.FantasyEntryId == user.FantasyEntryId);
                if (userFromDb == null)
                {
                    await db.AddAsync(user);
                }
                else
                {
                    user.UserId = userFromDb.UserId;
                    db.Update(user);
                }
            }

            return users;
        }

        public async Task<IList<UserLeague>> ImportUserLeagues(FantasyContext db, IList<User> users, League league, IList<UserLeague> userLeaguesFromDb)
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
                    await db.AddAsync(userLeague);
                }
                else
                {
                    userLeague.UserLeagueId = userLeagueFromDb.UserLeagueId;
                    db.Update(userLeague);
                }
            }

            return userLeagues;
        }

        public async Task<IList<PlayerEvent>> ImportPlayerEvents(FantasyContext db, 
            EventRootDTO eventRootDto, 
            Event finishedEvent, 
            IList<Player> players, 
            IList<PlayerEvent> playerEventsFromDb)
        {
            var playerEventsFromDbForEvent = playerEventsFromDb
                    .Where(pe => pe.Event.FantasyEventId == finishedEvent.FantasyEventId)
                    .ToList();

            var playerEvents = eventRootDto.elements.Select(e =>
                this._fantasyMapper.GetPlayerEvent(e, finishedEvent, players)
            ).ToList();

            foreach (var playerEvent in playerEvents)
            {
                var playerEventFromDb = playerEventsFromDbForEvent.FirstOrDefault(
                    pe => pe.Event.FantasyEventId == playerEvent.Event.FantasyEventId
                          && pe.Player.FantasyPlayerId == playerEvent.Player.FantasyPlayerId
                );

                if (playerEventFromDb == null)
                {
                    await db.AddAsync(playerEvent);
                }
                else
                {
                    playerEvent.PlayerEventId = playerEventFromDb.PlayerEventId;
                    db.Update(playerEvent);
                }
            }

            return playerEvents;
        }

        public async Task<IList<PlayerHeadToHeadEvent>> ImportHeadToHeadPlayerEvents(
            FantasyContext db, 
            HeadToHeadEventRootDTO headToHeadEventDto,
            Event finishedEvent,
            IList<Player> players,
            IList<PlayerHeadToHeadEvent> playerHeadToHeadEventsFromDb
        )
        {
            var playerHeadToHeadEventsFromDbForEvent = playerHeadToHeadEventsFromDb
                .Where(pe => pe.Event.FantasyEventId == finishedEvent.FantasyEventId)
                .ToList();

            var playerHeadToHeadEvents = headToHeadEventDto.results.Select(r =>
                this._fantasyMapper.GetPlayerHeadToHeadEvent(r, finishedEvent, players)
            ).ToList();

            foreach (var playerHeadToHeadEvent in playerHeadToHeadEvents)
            {
                var playerEventFromDb = playerHeadToHeadEventsFromDbForEvent.FirstOrDefault(
                    pe => pe.Event.FantasyEventId == playerHeadToHeadEvent.Event.FantasyEventId
                          && (pe.Entry1_Player.FantasyPlayerId == playerHeadToHeadEvent.Entry1_Player.FantasyPlayerId
                            || pe.Entry2_Player.FantasyPlayerId == playerHeadToHeadEvent.Entry2_Player.FantasyPlayerId)
                );

                if (playerEventFromDb == null)
                {
                    await db.AddAsync(playerHeadToHeadEvent);
                }
                else
                {
                    playerHeadToHeadEvent.PlayerHeadToHeadEventID = playerEventFromDb.PlayerHeadToHeadEventID;
                    db.Update(playerHeadToHeadEvent);
                }
            }

            return playerHeadToHeadEvents;
        }

        public async Task<Lineup> ImportLineup(FantasyContext db, Event finishedEvent, User user, IList<Lineup> lineupsFromDb)
        {
            var lineupFromDb = lineupsFromDb.FirstOrDefault(l => l.FkEventId == finishedEvent.EventId && l.FkUserId == user.UserId);

            var lineup = this._fantasyMapper.GetLineup(finishedEvent, user);

            if (lineupFromDb == null)
            {
                await db.AddAsync(lineup);
            }
            else
            {
                lineup.LineupId = lineupFromDb.LineupId;
                db.Update(lineup);
            }

            return lineup;
        }

        public async Task<IList<PlayerEventLineup>> ImportPlayerEventLineup(FantasyContext db, PicksRootDTO picksRoot, Event finishedEvent, Lineup lineup, IList<PlayerEvent> playerEvents, IList<PlayerEventLineup> playerEventLineupsFromDb)
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
                    await db.AddAsync(playerEventLineup);
                }
                else
                {
                    playerEventLineup.PlayerEventLineupId = playerLineupFromDb.PlayerEventLineupId;
                    db.Update(playerEventLineup);
                }
            }

            return playerEventLineups;
        }
    }
}
