using Microsoft.EntityFrameworkCore;
using Milabowl.Domain.Entities.Fantasy;
using Milabowl.Domain.Import;
using Milabowl.Infrastructure.Contexts;

namespace Milabowl.Infrastructure.Repositories
{
    public class ImportRepository: IImportRepository
    {
        private readonly FantasyContext _db;

        public ImportRepository(FantasyContext context)
        {
            this._db = context;
        }

        public async Task<IList<Event>> GetEvents()
        {
            return await this._db.Events.AsNoTracking().ToListAsync();
        }

        public async Task<IList<Team>> GetTeams()
        {
            return await this._db.Teams.AsNoTracking().ToListAsync();
        }

        public async Task<IList<Fixture>> GetFixtures()
        {
            return await this._db.Fixtures.AsNoTracking().ToListAsync();
        }

        public async Task<IList<Player>> GetPlayers()
        {
            return await this._db.Players.AsNoTracking().ToListAsync();
        }

        public async Task<IList<League>> GetLeagues()
        {
            return await this._db.Leagues.AsNoTracking().ToListAsync();
        }

        public async Task<IList<User>> GetUsers()
        {
            return await this._db.Users.AsNoTracking().ToListAsync();
        }

        public async Task<IList<Lineup>> GetLineups()
        {
            return await this._db.Lineups.AsNoTracking().ToListAsync();
        }

        public async Task<IList<UserLeague>> GetUserLeagues()
        {
            return await this._db.UserLeagues
                .Include(u => u.User)
                .Include(u => u.League)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IList<PlayerEvent>> GetPlayerEvents()
        {
            return await this._db.PlayerEvents.AsNoTracking()
                .Include(e => e.Event)
                .Include(pe => pe.Player)
                .ToListAsync();
        }

        public async Task<IList<UserHeadToHeadEvent>> GetUserHeadToHeadEvents()
        {
            return await this._db.UserHeadToHeadEvents.AsNoTracking()
                .Include(uh => uh.Event)
                .Include(uh => uh.User)
                .ToListAsync();
        }

        public async Task<IList<PlayerEventLineup>> GetPlayerEventLineups()
        {
            return await this._db.PlayerEventLineups
                .Include(pel => pel.PlayerEvent)
                .Include(pel => pel.Lineup)
                .ThenInclude(l => l.Event)
                .Include(pel => pel.Lineup)
                .ThenInclude(l => l.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(object entity)
        {
            await _db.AddAsync(entity);
        }

        public void Update(object entity)
        {
            _db.Update(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
