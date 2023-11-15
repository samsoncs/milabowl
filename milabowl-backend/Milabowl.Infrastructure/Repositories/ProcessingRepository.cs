using Microsoft.EntityFrameworkCore;
using Milabowl.Domain.Entities.Fantasy;
using Milabowl.Domain.Entities.Milabowl;
using Milabowl.Domain.Processing;
using Milabowl.Infrastructure.Contexts;

namespace Milabowl.Infrastructure.Repositories
{
    public class ProcessingRepository: IProcessingRepository
    {
        private readonly FantasyContext _context;

        public ProcessingRepository(FantasyContext context)
        {
            _context = context;
        }

        public async Task<IList<Event>> GetEventsToProcess()
        {
            return await this._context.Events
                .Include(e => e.Lineups)
                    .ThenInclude(l => l.PlayerEventLineups)
                        .ThenInclude(pel => pel.PlayerEvent)
                .Include(e => e.Lineups)
                    .ThenInclude(l => l.User)
                .Include(e => e.PlayerHeadToHeadEvents)
                    .ThenInclude(h2h => h2h.User)
                .Where(e => e.Finished && e.DataChecked)
                .OrderBy(g => g.GameWeek)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<int> GetNumGameWeeks()
        {
            return await _context.Events.CountAsync();
        }

        public async Task<IList<User>> GetUserToProcess(Guid evtId)
        {
            return await this._context.Users
                .Include(u => u.Lineups)
                    .ThenInclude(l => l.PlayerEventLineups)
                    .ThenInclude(pel => pel.PlayerEvent)
                    .ThenInclude(pel => pel.Player)
                .Include(u => u.Lineups)
                    .ThenInclude(l => l.PlayerEventLineups)
                    .ThenInclude(pel => pel.PlayerEvent)
                    .ThenInclude(pel => pel.Event)
                .Include(u => u.Lineups)
                    .ThenInclude(l => l.Event)
                .Include(u => u.HeadToHeadEvents)
                    .ThenInclude(hu => hu.Event)
                .Where(u => u.Lineups.Any(l => l.Event.EventId == evtId))
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<string> GetUsernameDirectlyInFront(Random random, int gameWeek, string userName)
        {
            var scores = await _context
                .MilaGWScores
                .Where(m => m.GameWeek < gameWeek)
                .Select(m => new { m.UserName, Points = m.GWScore })
                .ToListAsync();
                
            var scoresByUser = scores.GroupBy(s => s.UserName).Select(grp => new {
                UserName = grp.Key,
                Score = grp.ToList().Sum(s => s.Points)
            }).OrderBy(s => s.Score).ToList();

            var userScore = scoresByUser.First(s => s.UserName == userName);
            var nextUser = scoresByUser.FirstOrDefault(s => s.Score > userScore.Score);
            
            if (nextUser is null)
            {
                nextUser = scoresByUser[^2];
            }

            var nextUsers = scoresByUser.Where(s => s.Score == nextUser.Score).ToList();
            return nextUsers[random.Next(0, nextUsers.Count - 1)].UserName;
        }

        public async Task<IList<Player>> GetPlayersForGw(IList<Player> players)
        {
            var playerIds = players.Select(p => p.PlayerId).ToList();
            return await _context.Players
                .Include(p => p.PlayerEvents)
                .ThenInclude(p => p.Event)
                .Where(p => playerIds.Contains(p.PlayerId))
                .ToListAsync();
        }

        public async Task<bool> IsEventAlreadyCalculated(string eventName, string userEntryName)
        {
            return await this._context.MilaGWScores.AsNoTracking().AnyAsync(m =>
                m.GW == eventName &&
                m.TeamName == userEntryName);
        }

        public async Task<(Player? mostTradedIn, Player? mostTradedOut)> GetMostTradedPlayers(Guid eventId)
        {
            var mostTradedInPlayer = await this._context.PlayerEvents
                .Where(pe => pe.FkEventId == eventId)
                .OrderByDescending(pe => pe.TransfersIn)
                .Select(pe => pe.Player)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            var mostTradedOutPlayer = await this._context.PlayerEvents
                .Where(pe => pe.FkEventId == eventId)
                .OrderByDescending(pe => pe.TransfersOut)
                .Select(pe => pe.Player)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return (mostTradedInPlayer, mostTradedOutPlayer);
        }

        public async Task<UserHeadToHeadEvent?> GetOpponentHeadToHead(int userHeadToHeadEventId, Guid userId)
        {
            return await _context.UserHeadToHeadEvents.AsNoTracking().FirstOrDefaultAsync(h =>
                h.FantasyUserHeadToHeadEventID == userHeadToHeadEventId
                && h.FkUserId != userId);
        }

        public async Task AddMilaGwScores(IList<MilaGWScore> milaGwScores)
        {
            await _context.AddRangeAsync(milaGwScores);
            await _context.SaveChangesAsync();
        }
    }
}
