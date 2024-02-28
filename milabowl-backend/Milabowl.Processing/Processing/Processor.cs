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
                var rulesResults = _rulesProcessor.CalculateForUserGameWeek(userGameWeek);
                var totalMilaScore = rulesResults.Sum(r => r.Points);

                var rules = rulesResults.ToDictionary(
                    k => k.RuleName,
                    e => new RuleResult(e.Points, e.RuleShortName)
                );

                var result = new MilaResult(
                    userGameWeek.Event.Name,
                    totalMilaScore,
                    userGameWeek.User.TeamName,
                    userGameWeek.User.UserName,
                    userGameWeek.User.Id,
                    1,
                    userGameWeek.Event.GameWeek,
                    1,
                    1,
                    1,
                    1,
                    1,
                    rules
                );

                results.Add(result);
                var json = JsonSerializer.Serialize(result);
            }
        }

        var res = new MilaResults
        {
            OverallScore = new List<MilaResult>(),
            ResultsByUser = new List<UserResults>(),
            ResultsByWeek = new List<GameWeekResults>()
        };
        Console.WriteLine("Mila points processing complete");
    }
}
