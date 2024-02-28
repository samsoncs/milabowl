using System.Text.Json;
using Milabowl.Processing.DataImport;

namespace Milabowl.Processing.Processing;

public class Processor
{
    private readonly FplImporter _importer;
    private readonly IRulesProcessor _rulesProcessor;

    public Processor(FplImporter importer, IRulesProcessor rulesProcessor)
    {
        _rulesProcessor = rulesProcessor;
        _importer = importer;
    }

    public async Task ProcessMilaPoints()
    {
        Console.WriteLine("Starting to import FPL data");
        var userGameWeeksByGameWeek = await _importer.Import();
        Console.WriteLine("FPL data imported");

        Console.WriteLine("Starting to process mila points");

        var results = new List<MilaResult>();

        foreach (var gameWeek in userGameWeeksByGameWeek.Keys)
        {
            foreach (var userGameWeek in userGameWeeksByGameWeek[gameWeek])
            {
                // var rulesResults = _rulesProcessor.CalculateForUserGameWeek(userGameWeek);
                // var totalMilaScore = rulesResults.Sum(r => r.Points);

                var result = new MilaResult(
                    userGameWeek.Event.Name,
                    userGameWeek.TotalMilaScore,
                    userGameWeek.User.TeamName,
                    userGameWeek.User.UserName,
                    userGameWeek.User.Id,
                    userGameWeek.GwPosition,
                    userGameWeek.Event.GameWeek,
                    userGameWeek.CumulativeTotalMilaScore,
                    userGameWeek.AvgCumulativeTotalMilaScore,
                    userGameWeek.TotalCumulativeAverageMilaScore,
                    userGameWeek.MilaRank,
                    userGameWeek.MilaRankLastWeek,
                    userGameWeek.Rules
                );

                results.Add(result);
                var json = JsonSerializer.Serialize(result);
            }
        }

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

        var lastGameWeek = results.Max(r => r.GameWeek);

        var res = new MilaResults
        {
            OverallScore = results.Where(r => r.GameWeek == lastGameWeek).ToList(),
            ResultsByUser = resultsByUser,
            ResultsByWeek = resultsByWeek
        };
        Console.WriteLine("Mila points processing complete");
    }
}
