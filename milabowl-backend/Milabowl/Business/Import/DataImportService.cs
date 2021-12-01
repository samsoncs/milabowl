using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Milabowl.Infrastructure.Contexts;
using Milabowl.Infrastructure.Models;

namespace Milabowl.Business.Import
{
    public interface IDataImportService
    {
        public Task ImportData();
    }

    public class DataImportService: IDataImportService
    {
        private readonly IDataImportBusiness _dataImportBusiness;
        private readonly IDataImportProvider _dataImportProvider;
        private readonly FantasyContext _db;

        public DataImportService(IDataImportBusiness dataImportBusiness, IDataImportProvider dataImportProvider, FantasyContext db)
        {
            this._dataImportBusiness = dataImportBusiness;
            this._dataImportProvider = dataImportProvider;
            this._db = db;
        }

        public async Task ImportData()
        {
            var eventsFromDb = await this._db.Events.AsNoTracking().ToListAsync();
            var teamsFromDb = await this._db.Teams.AsNoTracking().ToListAsync();
            var playersFromDb = await this._db.Players.AsNoTracking().ToListAsync();
            var leaguesFromDb = await this._db.Leagues.AsNoTracking().ToListAsync();
            var usersFromDb = await this._db.Users.AsNoTracking().ToListAsync();
            var lineupsFromDb = await this._db.Lineups.AsNoTracking().ToListAsync();

            var userLeaguesFromDb = await this._db.UserLeagues
                .Include(u => u.User)
                .Include(u => u.League)
                .AsNoTracking()
                .ToListAsync();

            var playerEventsFromDb = await this._db.PlayerEvents.AsNoTracking()
                    .Include(e => e.Event)
                    .Include(pe => pe.Player)
                    .ToListAsync();

            var userHeadToHeadEventsFromDb = await this._db.UserHeadToHeadEvents.AsNoTracking()
                .Include(uh => uh.Event)
                .Include(uh => uh.User)
                .ToListAsync();

            var playerEventLineupsFromDb = await this._db.PlayerEventLineups
                .Include(pel => pel.PlayerEvent)
                .Include(pel => pel.Lineup)
                .ThenInclude(l => l.Event)
                .Include(pel => pel.Lineup)
                .ThenInclude(l => l.User)
                .AsNoTracking()
                .ToListAsync();

            var bootstrapRoot = await this._dataImportProvider.GetBootstrapRoot();
            var events = await this._dataImportBusiness.ImportEvents(this._db, bootstrapRoot, eventsFromDb);
            var teams = await this._dataImportBusiness.ImportTeams(this._db, bootstrapRoot, teamsFromDb);
            var players = await this._dataImportBusiness.ImportPlayers(this._db, bootstrapRoot, teams, playersFromDb);
            var fixtures = await this._dataImportProvider.GetFixtures();

            var playerHistoryRoots = players.Select(async p => await this._dataImportProvider.GetPlayerHistoryRoot(p.FantasyPlayerId))
                .Select(t => t.Result)
                .Where(t => t != null)
                .ToList();

            var leagueRoot = await this._dataImportProvider.GetLeagueRoot();
            var league = await this._dataImportBusiness.ImportLeague(this._db, leagueRoot, leaguesFromDb);
            var users = await this._dataImportBusiness.ImportUsers(this._db, leagueRoot, usersFromDb);
            await this._dataImportBusiness.ImportUserLeagues(this._db, users, league, userLeaguesFromDb);
            await this._db.SaveChangesAsync();

            foreach (var finishedEvent in events.Where(e => e.Finished && e.DataChecked))
            {
                if (playerEventsFromDb.Any(pe => pe.Event.GameWeek == finishedEvent.GameWeek))
                {
                    continue;
                }

                var eventRootDto = await this._dataImportProvider.GetEventRoot(finishedEvent.FantasyEventId);
                var playerEvents = await this._dataImportBusiness.ImportPlayerEvents(this._db, eventRootDto, finishedEvent, players, playerEventsFromDb, playerHistoryRoots, fixtures);

                var headToHeadEventRootDto = await this._dataImportProvider.GetHead2HeadEventRoot(finishedEvent.FantasyEventId);
                await this._dataImportBusiness.ImportUserHeadToHeadEvents(this._db, headToHeadEventRootDto, finishedEvent, users, userHeadToHeadEventsFromDb);

                foreach (var user in users)
                {
                    var picksRoot = await this._dataImportProvider.GetPicksRoot(finishedEvent.FantasyEventId, user.FantasyEntryId);
                    if (picksRoot.picks == null)
                    {
                        continue;
                    }

                    var lineup = await this._dataImportBusiness.ImportLineup(this._db, finishedEvent, user, lineupsFromDb);
                    await this._dataImportBusiness.ImportPlayerEventLineup(this._db, picksRoot, finishedEvent, lineup, playerEvents, playerEventLineupsFromDb);
                }

                await this._db.SaveChangesAsync();
            }
        }
    }
}
