using System.Collections.ObjectModel;
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

        var rules = ruleResults.ToDictionary(
            r => r.RuleName,
            r => new RuleResult(r.Points, r.RuleShortName)
        );

        return new MilaResult(
            userGameWeek.User.Event.Name,
            totalScore,
            userGameWeek.User.User.TeamName,
            userGameWeek.User.User.UserName,
            userGameWeek.User.User.Id,
            1, //userGameWeek.User.Position.GwPosition,
            userGameWeek.User.Event.GameWeek,
            1, //userGameWeek.User.MilaScores.CumulativeTotalMilaScore,
            1, //userGameWeek.MilaScores.AvgCumulativeTotalMilaScore,
            1, //userGameWeek.MilaScores.TotalCumulativeAvgMilaScore,
            1, //userGameWeek.Position.MilaRank,
            1, //userGameWeek.Position.MilaRankLastWeek,
            // new ReadOnlyDictionary<string, RuleResult>(rules)
            ruleResults
        );
    }
}
