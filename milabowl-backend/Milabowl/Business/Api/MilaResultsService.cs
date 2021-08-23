using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Milabowl.Business.DTOs.Api;
using Milabowl.Infrastructure.Contexts;

namespace Milabowl.Business.Api
{
    public interface IMilaResultsService
    {
        Task<MilaResultsDTO> GetMilaResults();
    }

    public class MilaResultsService: IMilaResultsService
    {
        private readonly IMilaResultsBusiness _milaResultsBusiness;
        private readonly FantasyContext _db;

        public MilaResultsService(IMilaResultsBusiness milaResultsBusiness, FantasyContext db)
        {
            this._milaResultsBusiness = milaResultsBusiness;
            this._db = db;
        }

        public async Task<MilaResultsDTO> GetMilaResults()
        {
            var gameWeekScores = await this._db.MilaGWScores
                .OrderBy(m => m.GameWeek)
                .ToListAsync();

            var milaResultDtos = gameWeekScores
                .Select(m =>
                {
                    var gameWeeksUpToNow = this._milaResultsBusiness.GetGameWeekScoresUpToNow(gameWeekScores, m);
                    var gameWeeksForPlayerUpToNow = this._milaResultsBusiness.GetGameWeekScoresForPlayerUpToNow(gameWeekScores, m);
                    var milaPositionThisGameWeek = this._milaResultsBusiness.GetPositionThisGameWeek(gameWeeksUpToNow, m);
                    var milaPositionLastGameWeek = this._milaResultsBusiness.GetPositionLastGameWeek(gameWeeksUpToNow, m);
                    return this._milaResultsBusiness.GetMilaResultDTO(m, gameWeeksForPlayerUpToNow, gameWeeksUpToNow, milaPositionThisGameWeek, milaPositionLastGameWeek);
                }).ToList();

            var resultsByWeek = milaResultDtos
                .GroupBy(m => m.GameWeek)
                .Select(grp => new GameWeekDTO
                {
                    GameWeek = grp.Key,
                    Results = grp.OrderByDescending(g => g.CumulativeMilaPoints).ToList()
                }).ToList();

            var resultsByUser = milaResultDtos
                .GroupBy(m => m.TeamName)
                .Select(grp => new UserResultsDTO
                {
                    TeamName = grp.Key,
                    Results = grp.OrderBy(g => g.GameWeek).ToList()
                }).ToList();

            var overall = this._milaResultsBusiness.GetOverallScore(milaResultDtos);

            return new MilaResultsDTO
            {
                ResultsByWeek = resultsByWeek,
                ResultsByUser = resultsByUser,
                OverallScore = overall
            };
        }
    }
}
