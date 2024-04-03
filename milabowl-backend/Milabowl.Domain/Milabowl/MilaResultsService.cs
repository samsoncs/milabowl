﻿using Milabowl.Domain.Milabowl.DTOs;

namespace Milabowl.Domain.Milabowl;

public record FplPlayerEventResult(
    string LastName,
    string TeamName,
    int Points,
    string Position,
    bool IsCap,
    bool IsViceCap,
    bool IsBench
);

public record FplUserGameWeekResult(
    int GameWeek,
    string TeamName,
    int TotalScore,
    IList<FplPlayerEventResult> Lineup
);

public record FplResults(
    IList<FplUserGameWeekResult> Results
);

public interface IMilaResultsService
{
    Task<MilaResults> GetMilaResults();
    Task<FplResults> GetFplResults();
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

    public async Task<MilaResults> GetMilaResults()
    {
        var gameWeekScores = await _milaRepository.GetMilaGwScores();

        var milaResults = gameWeekScores
            .Select(m =>
            {
                var gameWeeksUpToNow = this._milaResultsBusiness.GetGameWeekScoresUpToNow(gameWeekScores, m);
                var gameWeeksForPlayerUpToNow = this._milaResultsBusiness.GetGameWeekScoresForPlayerUpToNow(gameWeekScores, m);
                var milaPositionThisGameWeek = this._milaResultsBusiness.GetPositionThisGameWeek(gameWeeksUpToNow, m);
                var milaPositionLastGameWeek = this._milaResultsBusiness.GetPositionLastGameWeek(gameWeeksUpToNow, m);
                return this._milaResultsBusiness.GetMilaResult(m, gameWeeksForPlayerUpToNow, gameWeeksUpToNow, milaPositionThisGameWeek, milaPositionLastGameWeek);
            }).ToList();

        var resultsByWeek = milaResults
            .GroupBy(m => m.GameWeek)
            .Select(grp => new GameWeekResults
            {
                GameWeek = grp.Key,
                Results = grp.OrderByDescending(g => g.CumulativeMilaPoints).ToList()
            }).ToList();

        var resultsByUser = milaResults
            .GroupBy(m => m.TeamName)
            .Select(grp => new UserResults
            {
                TeamName = grp.Key,
                Results = grp.OrderBy(g => g.GameWeek).ToList()
            }).ToList();

        var overall = this._milaResultsBusiness.GetOverallScore(milaResults);

        return new MilaResults
        {
            ResultsByWeek = resultsByWeek,
            ResultsByUser = resultsByUser,
            OverallScore = overall
        };
    }

    public async Task<FplResults> GetFplResults()
    {
        var fplResults = await _milaRepository.GetFplResults();
        return fplResults;
    }
}