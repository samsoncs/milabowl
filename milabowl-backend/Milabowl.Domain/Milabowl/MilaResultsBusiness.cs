using Milabowl.Domain.Entities.Milabowl;
using Milabowl.Domain.Milabowl.DTOs;

namespace Milabowl.Domain.Milabowl;

public interface IMilaResultsBusiness
{
    List<MilaResult> GetOverallScore(IList<MilaResult> milaResults);
    List<MilaGWScore> GetGameWeekScoresUpToNow(IList<MilaGWScore> gameWeekScores, MilaGWScore m);
    List<MilaGWScore> GetGameWeekScoresForPlayerUpToNow(IList<MilaGWScore> gameWeekScores, MilaGWScore m);
    int GetPositionThisGameWeek(IList<MilaGWScore> gameWeekScoresUpToNow, MilaGWScore m);
    int? GetPositionLastGameWeek(IList<MilaGWScore> gameWeekScoresUpToNow, MilaGWScore m);
    MilaResult GetMilaResult(MilaGWScore m, IList<MilaGWScore> gameWeeksForPlayerUpToNow, IList<MilaGWScore> gameWeeksUpToNow, int milaPositionThisWeek, int? milaPositionLastWeek);
}

public class MilaResultsBusiness: IMilaResultsBusiness
{

    public List<MilaResult> GetOverallScore(IList<MilaResult> milaResults)
    {
        return milaResults
            .GroupBy(m => m.TeamName)
            .SelectMany(t =>
                t.ToList()
                    .GroupBy(g => 1)
                    .Select(s =>
                    {
                        var lastResult = milaResults.Where(m => m.TeamName == t.Key).OrderByDescending(m => m.GameWeek).FirstOrDefault();
                        return new MilaResult
                        {
                            MilaPoints = new MilaRulePoints
                            {
                                RedCard = s.Sum(a => a.MilaPoints.RedCard),
                                GWPositionScore = s.Sum(a => a.MilaPoints.GWPositionScore),
                                YellowCard = s.Sum(a => a.MilaPoints.YellowCard),
                                CapFail = s.Sum(a => a.MilaPoints.CapFail),
                                BenchFail = s.Sum(a => a.MilaPoints.BenchFail),
                                CapKeep = s.Sum(a => a.MilaPoints.CapKeep),
                                CapDef = s.Sum(a => a.MilaPoints.CapDef),
                                MinusIsPlus = s.Sum(a => a.MilaPoints.MinusIsPlus),
                                IncreaseStreak = s.Sum(a => a.MilaPoints.IncreaseStreak),
                                EqualStreak = s.Sum(a => a.MilaPoints.EqualStreak),
                                GW69 = s.Sum(a => a.MilaPoints.GW69),
                                HeadToHeadMeta = s.Sum(a => a.MilaPoints.HeadToHeadMeta),
                                SixtyNineSub = s.Sum(a => a.MilaPoints.SixtyNineSub),
                                UniqueCap = s.Sum(a => a.MilaPoints.UniqueCap),
                                TrendyBitch = s.Sum(a => a.MilaPoints.TrendyBitch),
                                Mushroom = s.Sum(a => a.MilaPoints.Mushroom),
                                RedShell = s.Sum(a => a.MilaPoints.RedShell),
                                GreenShell = s.Sum(a => a.MilaPoints.GreenShell),
                                Banana = s.Sum(a => a.MilaPoints.Banana),
                                BombPoints = s.Sum(a => a.MilaPoints.BombPoints),
                                BombState = lastResult?.MilaPoints.BombState,
                                ActiveChip = lastResult?.MilaPoints.ActiveChip,
                                DarthMaulPoints = s.Sum(a => a.MilaPoints.DarthMaulPoints),
                                IsDarthMaul = lastResult?.MilaPoints.IsDarthMaul,
                                IsDarthMaulContender = lastResult?.MilaPoints.IsDarthMaulContender,
                                Total = lastResult?.MilaPoints.Total,
                            },
                            GWPosition = s.Sum(a => a.GWPosition),
                            GWScore = s.Sum(a => a.GWScore),
                            TeamName = t.Key,
                            UserName = lastResult?.UserName,
                            UserId = lastResult!.UserId,
                            MilaRank = lastResult?.MilaRank,
                            MilaRankLastWeek = lastResult?.MilaRankLastWeek,
                            CumulativeMilaPoints = lastResult?.CumulativeMilaPoints,
                        };
                    })
            ).OrderBy(m => m.MilaRank).ToList();
    }

    public List<MilaGWScore> GetGameWeekScoresUpToNow(IList<MilaGWScore> gameWeekScores, MilaGWScore m)
    {
        return gameWeekScores
            .Where(im => im.GameWeek <= m.GameWeek)
            .ToList();
    }

    public List<MilaGWScore> GetGameWeekScoresForPlayerUpToNow(IList<MilaGWScore> gameWeekScores, MilaGWScore m)
    {
        return this.GetGameWeekScoresUpToNow(gameWeekScores, m).Where(im => im.TeamName == m.TeamName)
            .ToList();
    }

    public int GetPositionThisGameWeek(IList<MilaGWScore> gameWeekScoresUpToNow, MilaGWScore m)
    {
        return gameWeekScoresUpToNow.Where(im => im.GameWeek == m.GameWeek)
            .OrderByDescending(im =>
                Math.Round(
                    gameWeekScoresUpToNow.Where(g => g.TeamName == im.TeamName)
                        .Sum(g => g.MilaPoints), 2
                )
            ).ToList()
            .FindIndex(i => i.TeamName == m.TeamName) + 1;
    }

    public int? GetPositionLastGameWeek(IList<MilaGWScore> gameWeekScoresUpToNow, MilaGWScore m)
    {
        return m.GameWeek < 2 ? (int?)null :
            (gameWeekScoresUpToNow.Where(im => im.GameWeek == (m.GameWeek - 1))
                .OrderByDescending(im =>
                    Math.Round(
                        gameWeekScoresUpToNow.Where(g => g.TeamName == im.TeamName && g.GameWeek < m.GameWeek)
                            .Sum(g => g.MilaPoints), 2
                    )
                ).ToList()
                .FindIndex(i => i.TeamName == m.TeamName) + 1);
    }

    public MilaResult GetMilaResult(MilaGWScore m, IList<MilaGWScore> gameWeeksForPlayerUpToNow, IList<MilaGWScore> gameWeeksUpToNow, int milaPositionThisWeek, int? milaPositionLastWeek)
    {
        return new MilaResult
        {
            MilaPoints = new MilaRulePoints
            {
                BenchFail = m.BenchFail,
                CapDef = m.CapDef,
                CapFail = m.CapFail,
                CapKeep = m.CapKeep,
                EqualStreak = m.EqualStreak,
                GW69 = m.GW69,
                RedCard = m.RedCard,
                YellowCard = m.YellowCard,
                MinusIsPlus = m.MinusIsPlus,
                IncreaseStreak = m.IncreaseStreak,
                GWPositionScore = m.GWPositionScore,
                HeadToHeadMeta = m.HeadToHeadMeta,
                SixtyNineSub = m.SixtyNineSub,
                UniqueCap = m.UniqueCap,
                TrendyBitch = m.TrendyBitch,
                Mushroom = m.Mushroom,
                RedShell = m.RedShell,
                GreenShell = m.GreenShell,
                Banana = m.Banana,
                BombPoints = m.BombPoints,
                BombState = m.BombState?.ToString(),
                ActiveChip = m.ActiveChip,
                DarthMaulPoints = m.DarthMaulPoints,
                IsDarthMaul = m.IsDarthMaul,
                IsDarthMaulContender = m.IsDarthMaulContender,
                Total = m.MilaPoints
            },

            GameWeek = m.GameWeek,
            GW = m.GW,
            GWPosition = m.GWPosition,
            GWScore = m.GWScore,
            TeamName = m.TeamName,
            UserName = m.UserName,
            UserId = m.UserId,
            CumulativeAverageMilaPoints = gameWeeksForPlayerUpToNow
                .Sum(g => Math.Round(g.MilaPoints / (decimal)gameWeeksForPlayerUpToNow.Count, 2)),
            CumulativeMilaPoints = Math.Round(gameWeeksForPlayerUpToNow
                .Sum(g => g.MilaPoints), 2),
            TotalCumulativeAverageMilaPoints = Math.Round(gameWeeksUpToNow
                .Sum(g => g.MilaPoints / (decimal)gameWeeksUpToNow.Count), 2),
            MilaRank = milaPositionThisWeek,
            MilaRankLastWeek = milaPositionLastWeek
        };
    }
}