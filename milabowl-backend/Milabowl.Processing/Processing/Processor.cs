using System.Text.Json;
using Milabowl.Processing.DataImport;

namespace Milabowl.Processing.Processing;

public class Processor
{
    private readonly FplImporter _importer;

    public Processor(FplImporter importer)
    {
        _importer = importer;
    }

    public async Task ProcessMilaPoints()
    {
        Console.WriteLine("Starting to import FPL data");
        var results = await _importer.Import();
        Console.WriteLine("FPL data imported");

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

        Console.WriteLine("Starting to process mila points");
        var resultsByWeek = summedMilaResults
            .GroupBy(m => m.GameWeek)
            .Select(grp => new GameWeekResults
            {
                GameWeek = grp.Key,
                Results = grp.OrderByDescending(g => g.CumulativeMilaPoints).ToList()
            })
            .ToList();

        var resultsByUser = summedMilaResults
            .GroupBy(m => m.TeamName)
            .Select(grp => new UserResults
            {
                TeamName = grp.Key,
                Results = grp.OrderBy(g => g.GameWeek).ToList()
            })
            .ToList();

        var lastGameWeek = summedMilaResults.Max(r => r.GameWeek);

        var res = new MilaResults
        {
            OverallScore = summedMilaResults
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

        var json = JsonSerializer.Serialize(
            res,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
        );
        Console.WriteLine("Mila points processing complete");
    }
}
