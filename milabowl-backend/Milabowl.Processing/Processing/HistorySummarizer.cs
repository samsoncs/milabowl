namespace Milabowl.Processing.Processing;

public class HistorySummarizer
{
    public IReadOnlyList<MilaResultDto> Summarize(IReadOnlyList<MilaResult> results)
    {
        var totalMilaPoints = new Dictionary<string, decimal>(
            results
                .Select(r => r.TeamName)
                .Distinct()
                .Select(s => new KeyValuePair<string, decimal>(s, 0))
        );

        var summedMilaResults = results
            .GroupBy(r => r.GameWeek)
            .SelectMany(g =>
            {
                foreach (var gr in g)
                {
                    totalMilaPoints[gr.TeamName] += gr.Rules.Sum(ru => ru.Points);
                }

                return g.OrderBy(v => v.GameWeek)
                    .Select(r =>
                    {
                        var milaRankThisWeek =
                            results.Count(v =>
                                v.TeamName != r.TeamName
                                && v.GameWeek == r.GameWeek
                                && totalMilaPoints[v.TeamName] > totalMilaPoints[r.TeamName]
                            ) + 1;

                        int? milaRankLastWeek =
                            r.GameWeek > 1
                                ? results.Count(v =>
                                    v.TeamName != r.TeamName
                                    && v.GameWeek == r.GameWeek
                                    && (totalMilaPoints[v.TeamName] - v.Rules.Sum(ru => ru.Points))
                                        > (
                                            totalMilaPoints[r.TeamName]
                                            - r.Rules.Sum(ru => ru.Points)
                                        )
                                ) + 1
                                : null;

                        var gwPosition =
                            results.Count(v =>
                                v.TeamName != r.TeamName
                                && v.GameWeek == r.GameWeek
                                && v.Rules.Sum(ru => ru.Points) > r.Rules.Sum(ru => ru.Points)
                            ) + 1;

                        return new MilaResultDto(
                            r.Gw,
                            r.GwScore,
                            r.TeamName,
                            r.UserName,
                            r.UserId,
                            gwPosition,
                            r.GameWeek,
                            milaRankThisWeek,
                            milaRankLastWeek,
                            Math.Round(totalMilaPoints[r.TeamName], 2),
                            Math.Round(totalMilaPoints[r.TeamName] / r.GameWeek, 2),
                            Math.Round(
                                totalMilaPoints.Sum(t => t.Value)
                                    / (r.GameWeek * totalMilaPoints.Count),
                                2
                            ),
                            r.Rules
                        );
                    });
            })
            .ToList();

        return summedMilaResults;
    }
}
