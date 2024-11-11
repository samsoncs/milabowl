namespace Milabowl.Processing.Processing;

public static class MilaResultMapper
{
    public static MilaResults MapToResult(this IReadOnlyList<MilaResultDto> results)
    {
        var resultsByWeek = results
            .GroupBy(m => m.GameWeek)
            .Select(grp => new GameWeekResults
            {
                GameWeek = grp.Key,
                Results = grp.OrderByDescending(g => g.CumulativeMilaPoints).ToList()
            })
            .ToList();

        var resultsByUser = results
            .GroupBy(m => m.TeamName)
            .Select(grp => new UserResults
            {
                TeamName = grp.Key,
                Results = grp.OrderBy(g => g.GameWeek).ToList()
            })
            .ToList();

        return new MilaResults
        {
            OverallScore = results
                .GroupBy(r => r.TeamName)
                .Select(s =>
                {
                    var last = s.OrderBy(r => r.GameWeek).Last();
                    return new OverallResult(
                        last.GwScore,
                        last.TeamName,
                        last.UserName,
                        last.UserId,
                        last.GwPosition,
                        last.GameWeek,
                        last.CumulativeMilaPoints,
                        last.CumulativeAverageMilaPoints,
                        last.CumulativeAverageMilaPoints,
                        last.MilaRank,
                        last.MilaRankLastWeek
                    );
                })
                .ToList(),
            ResultsByUser = resultsByUser,
            ResultsByWeek = resultsByWeek
        };
    }
}
