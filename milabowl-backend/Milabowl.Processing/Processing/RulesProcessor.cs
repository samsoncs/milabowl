using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing;

public interface IRulesProcessor
{
    MilaResult CalculateForUserGameWeek(ManagerGameWeekState userGameWeek);
}

public class RulesProcessor : IRulesProcessor
{
    private readonly IEnumerable<IMilaRule> _rules;
    public RulesProcessor(IEnumerable<IMilaRule> rules)
    {
        _rules = rules;
    }

    public MilaResult CalculateForUserGameWeek(ManagerGameWeekState userGameWeek)
    {
        var ruleResults = _rules.Select(r => r.Calculate(userGameWeek)).ToList();
        var totalScore = ruleResults.Sum(r => r.Points);
        return new MilaResult(
            userGameWeek.Event.Name,
            totalScore,
            userGameWeek.User.TeamName,
            userGameWeek.User.UserName,
            userGameWeek.User.Id,
            userGameWeek.Event.GameWeek,
            ruleResults
        );
    }
}
