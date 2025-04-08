using Microsoft.EntityFrameworkCore;
using Milabowl.Domain.Entities.Milabowl;
using Milabowl.Domain.Milabowl;
using Milabowl.Infrastructure.Contexts;

namespace Milabowl.Infrastructure.Repositories
{
    public class MilaRepository: IMilaRepository
    {
        private readonly FantasyContext _context;


        public MilaRepository(FantasyContext context)
        {
            _context = context;
        }

        public async Task<IList<MilaGWScore>> GetMilaGwScores()
        {
            return await _context.MilaGWScores
                .OrderBy(m => m.GameWeek)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<FplResults> GetFplResults()
        {
            var users = await _context.Users
                .Include(u => u.Lineups)
                .ThenInclude(l => l.PlayerEventLineups)
                .ThenInclude(pel => pel.PlayerEvent)
                .ThenInclude(playerEvent => playerEvent.Player)
                .ThenInclude(player => player.Team)
                .Include(u => u.Lineups)
                .ThenInclude(l => l.Event)
                .AsNoTracking()
                .ToListAsync();
            
            
            var results = users.SelectMany(u =>
            {
                return u.Lineups.Select(l =>
                {
                    var total = l.PlayerEventLineups
                        .Sum(pel => pel.PlayerEvent.TotalPoints * pel.Multiplier);
                    return new FplUserGameWeekResult(
                        l.Event.GameWeek,
                        u.EntryName,
                        total,
                        l.PlayerEventLineups.Select(pel => new FplPlayerEventResult(
                            pel.PlayerEvent.Player.WebName,
                            pel.PlayerEvent.Player.Team.TeamName,
                            pel.PlayerEvent.TotalPoints,
                            pel.PlayerEvent.Player.ElementType switch
                            {
                                1 => "GK",
                                2 => "DEF",
                                3 => "MID",
                                4 => "FWD",
                                5 => "MNG",
                                _ => throw new ArgumentException("Not possible")
                            },
                            pel.IsCaptain,
                            pel.IsViceCaptain,
                            pel.Multiplier == 0
                        )).Where(pel => pel.Position != "MNG").ToList()
                    );
                });
            });

            return new FplResults(results.ToList());
        }
    }
}
