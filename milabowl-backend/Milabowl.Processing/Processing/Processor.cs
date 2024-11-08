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

        Console.WriteLine("Starting to process mila points");
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

        var json = JsonSerializer.Serialize(
            res,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
        );
        Console.WriteLine("Mila points processing complete");
    }
}
