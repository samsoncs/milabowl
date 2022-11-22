using System;
using System.Linq;
using System.Threading.Tasks;
using Milabowl.Business.DTOs.Api;
using Milabowl.Repositories;

namespace Milabowl.Business.Api
{
    public interface IMilaResultsService
    {
        Task<MilaResultsDTO> GetMilaResults();
    }

    public class MilaResultsService: IMilaResultsService
    {
        private readonly IMilaResultsBusiness _milaResultsBusiness;
        private readonly IMilaRepository _milaRepository;

        public MilaResultsService(IMilaResultsBusiness milaResultsBusiness, IMilaRepository milaRepository)
        {
            this._milaResultsBusiness = milaResultsBusiness;
            _milaRepository = milaRepository;
        }

        public async Task<MilaResultsDTO> GetMilaResults()
        {
            var gameWeekScores = await _milaRepository.GetMilaGwScores();

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
