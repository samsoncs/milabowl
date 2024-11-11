using System.Text.Json;
using Milabowl.Processing.DataImport;
using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing;

public class Processor
{
    private readonly FplImporter _importer;
    private readonly HistorySummarizer _summarizer;
    private readonly IRulesProcessor _rulesProcessor;
    private readonly JsonSerializerOptions _jsonOptions =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public Processor(
        FplImporter importer,
        HistorySummarizer summarizer,
        IRulesProcessor rulesProcessor
    )
    {
        _importer = importer;
        _summarizer = summarizer;
        _rulesProcessor = rulesProcessor;
    }

    public async Task ProcessMilaPoints()
    {
        Console.WriteLine("Importing FPL data");
        var importedGameWeekStates = await _importer.Import();
        Console.WriteLine("Importing FPL data - Finished");
        Console.WriteLine("Processing rules");
        var results = ProcessRules(importedGameWeekStates);
        Console.WriteLine("Processing rules - Finished");
        var summarizedMilaResults = _summarizer.Summarize(results);
        var json = JsonSerializer.Serialize(summarizedMilaResults.MapToResult(), _jsonOptions);
        Console.WriteLine("Mila points processing complete");
    }

    private IReadOnlyList<MilaResult> ProcessRules(
        IReadOnlyList<MilaGameWeekState> importedGameWeekStates
    )
    {
        return importedGameWeekStates
            .Select(m => _rulesProcessor.CalculateForUserGameWeek(m))
            .ToList()
            .AsReadOnly();
    }
}
