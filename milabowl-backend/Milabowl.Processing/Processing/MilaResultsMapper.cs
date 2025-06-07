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
                    var summedResults = s.SelectMany(r => r.Rules)
                        .GroupBy(r => r.RuleShortName)
                        .Select(a =>
                        {
                            var first = a.First();
                            return first with { Points = a.Sum(aa => aa.Points) };
                        })
                        .ToList();

                    return new OverallResult(
                        last.GwScore,
                        last.TeamName,
                        last.Gw,
                        last.UserName,
                        last.UserId,
                        last.GwPosition,
                        last.GameWeek,
                        last.CumulativeMilaPoints,
                        last.CumulativeAverageMilaPoints,
                        last.CumulativeAverageMilaPoints,
                        last.MilaRank,
                        last.MilaRankLastWeek,
                        summedResults
                    );
                })
                .ToList(),
            ResultsByUser = resultsByUser,
            ResultsByWeek = resultsByWeek,
            Rules = resultsByWeek.FirstOrDefault()?.Results.FirstOrDefault()?.Rules.Select(r => new Rule(r.RuleName, r.RuleShortName, r.Description))?.ToList() ??
                    []
        };
    }
}
