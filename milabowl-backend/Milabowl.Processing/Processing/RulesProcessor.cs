using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing;

public interface IRulesProcessor
{
    MilaResult CalculateForUserGameWeek(MilaGameWeekState userGameWeek);
}

public class RulesProcessor : IRulesProcessor
{
    private readonly IEnumerable<IMilaRule> _rules;

    public RulesProcessor(IEnumerable<IMilaRule> rules)
    {
        _rules = rules;
    }

    public MilaResult CalculateForUserGameWeek(MilaGameWeekState userGameWeek)
    {
        var ruleResults = _rules.Select(r => r.Calculate(userGameWeek)).ToList();
        var totalScore = ruleResults.Sum(r => r.Points);
        return new MilaResult(
            userGameWeek.User.Event.Name,
            totalScore,
            userGameWeek.User.User.TeamName,
            userGameWeek.User.User.UserName,
            userGameWeek.User.User.Id,
            userGameWeek.User.Event.GameWeek,
            ruleResults
        );
    }
}
