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
    }
}
