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
        foreach (var gameWeek in userGameWeeksByGameWeek.Keys)
        {
            foreach (var userGameWeek in userGameWeeksByGameWeek[gameWeek])
            {
                var results = _rulesProcessor.CalculateForUserGameWeek(userGameWeek);
                var totalMilaScore = results.Sum(r => r.Points);
            }
        }
        Console.WriteLine("Mila points processing complete");

    }
}
