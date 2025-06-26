using System.Text.Json;
using System.Text.Json.Serialization;
using Milabowl.Processing.DataImport;
using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing;

public class Processor
{
    private readonly FplImporter _importer;
    private readonly HistorySummarizer _summarizer;
    private readonly IRulesProcessor _rulesProcessor;
    private readonly IBombState _bombState;
    private readonly JsonSerializerOptions _jsonOptions =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, Converters = { new JsonStringEnumConverter() }};

    public Processor(
        FplImporter importer,
        HistorySummarizer summarizer,
        IRulesProcessor rulesProcessor, IBombState bombState)
    {
        _importer = importer;
        _summarizer = summarizer;
        _rulesProcessor = rulesProcessor;
        _bombState = bombState;
    }

    public async Task ProcessMilaPoints(string filePath)
    {
        Console.WriteLine("Importing FPL data for rules processing");
        var importedGameWeekStates = await _importer.ImportFplDataForRulesProcessing();
        Console.WriteLine("Importing FPL data for rules processing - Finished");
        Console.WriteLine("Processing rules");
        var results = ProcessRules(importedGameWeekStates);
        Console.WriteLine("Processing rules - Finished");
        var summarizedMilaResults = _summarizer.Summarize(results);
        var json = JsonSerializer.Serialize(summarizedMilaResults.MapToResult(), _jsonOptions);
        await File.WriteAllTextAsync($"{filePath}/game_state.json", json);
        var bombState = _bombState.GetBombState();
        var bombStateJson = JsonSerializer.Serialize(bombState, _jsonOptions);
        await File.WriteAllTextAsync($"{filePath}/bomb_state.json", bombStateJson);
        Console.WriteLine("Mila points processing complete");
        Console.WriteLine("Importing FPL data");
        var fplData = await _importer.ImportFplData();
        var fplJson = JsonSerializer.Serialize(fplData, _jsonOptions);
        await File.WriteAllTextAsync($"{filePath}/fpl_state.json", fplJson);
        Console.WriteLine("FPL data import - Finished");
    }

    private IReadOnlyList<MilaResult> ProcessRules(
        IReadOnlyList<ManagerGameWeekState> importedGameWeekStates
    )
    {
        return importedGameWeekStates
            .Select(m => _rulesProcessor.CalculateForUserGameWeek(m))
            .ToList()
            .AsReadOnly();
    }
}
