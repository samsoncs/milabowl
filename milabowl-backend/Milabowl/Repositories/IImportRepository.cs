using Milabowl.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Milabowl.Repositories
{
    public interface IImportRepository
    {
        Task<IList<Event>> GetEvents();

        public Task<IList<Team>> GetTeams();
        public Task<IList<Fixture>> GetFixtures();
        public Task<IList<Player>> GetPlayers();
        public Task<IList<League>> GetLeagues();
        public Task<IList<User>> GetUsers();
        public Task<IList<Lineup>> GetLineups();
        public Task<IList<UserLeague>> GetUserLeagues();
        public Task<IList<PlayerEvent>> GetPlayerEvents();
        public Task<IList<UserHeadToHeadEvent>> GetUserHeadToHeadEvents();
        public Task<IList<PlayerEventLineup>> GetPlayerEventLineups();

        public Task AddAsync(object entity);
        public void Update(object entity);
        public Task SaveChangesAsync();
    }
}
