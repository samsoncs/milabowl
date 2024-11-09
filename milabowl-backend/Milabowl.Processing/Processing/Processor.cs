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

        var summedMilaResults = results
            .GroupBy(r => r.TeamName)
            .SelectMany(g =>
            {
                var cumulativeMilaPoints = 0m;
                return g.OrderBy(v => v.GameWeek)
                    .Select(r =>
                    {
                        var milaRankThisWeek =
                            results.Count(v =>
                                v.TeamName != r.TeamName
                                && v.GameWeek == r.GameWeek
                                && v.Rules.Sum(ru => ru.Points) > r.Rules.Sum(ru => ru.Points)
                            ) + 1;

                        int? milaRankLastWeek =
                            r.GameWeek > 1
                                ? results.Count(v =>
                                    v.TeamName != r.TeamName
                                    && v.GameWeek == r.GameWeek - 1
                                    && v.Rules.Sum(ru => ru.Points) > r.Rules.Sum(ru => ru.Points)
                                ) + 1
                                : null;

                        var gwPosition =
                            results.Count(v =>
                                v.TeamName != r.TeamName
                                && v.GameWeek == r.GameWeek
                                && v.GwScore > r.GwScore
                            ) + 1;

                        cumulativeMilaPoints += r.Rules.Sum(v => v.Points);
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
                            cumulativeMilaPoints,
                            cumulativeMilaPoints / r.GameWeek,
                            cumulativeMilaPoints / r.GameWeek,
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
