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
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() },
    };

    public Processor(
        FplImporter importer,
        HistorySummarizer summarizer,
        IRulesProcessor rulesProcessor,
        IBombState bombState
    )
    {
        _importer = importer;
        _summarizer = summarizer;
        _rulesProcessor = rulesProcessor;
        _bombState = bombState;
    }

    public async Task ProcessMilaPoints()
    {
        var filePath = FilePathResolver.ResolveGameStateFilePath();
        Console.WriteLine("Importing FPL data for rules processing");
        var importData = await _importer.ImportFplDataForRulesProcessing();
        Console.WriteLine("Importing FPL data for rules processing - Finished");
        Console.WriteLine("Processing rules");
        var results = ProcessRules(importData.ManagerGameWeekStates);
        Console.WriteLine("Processing rules - Finished");
        var summarizedMilaResults = _summarizer.Summarize(results);
        var json = JsonSerializer.Serialize(
            summarizedMilaResults.MapToResult(importData.IsLive),
            _jsonOptions
        );
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

public static class FilePathResolver
{
    public static string ResolveGameStateFilePath()
    {
        var gitRoot = FindGitRoot(AppContext.BaseDirectory);
        var gameStatePath = Path.Combine(gitRoot, "milabowl-astro", "src", "game_state");
        return gameStatePath;
    }

    private static string FindGitRoot(string startDir)
    {
        var dir = new DirectoryInfo(startDir);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, ".git", "config")))
        {
            dir = dir.Parent;
        }
        return dir?.FullName ?? throw new DirectoryNotFoundException("Git root not found.");
    }
}
