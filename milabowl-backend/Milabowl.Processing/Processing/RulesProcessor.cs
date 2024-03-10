using Milabowl.Processing.DataImport;
using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing;

public interface IRulesProcessor
{
    List<MilaRuleResult> CalculateForUserGameWeek(MilaGameWeekState userGameWeek);
}

public class RulesProcessor : IRulesProcessor
{
    private readonly IEnumerable<IMilaRule> _rules;

    public RulesProcessor(IEnumerable<IMilaRule> rules)
    {
        _rules = rules;
    }

    public List<MilaRuleResult> CalculateForUserGameWeek(MilaGameWeekState userGameWeek)
    {
        return _rules.Select(r => r.Calculate(userGameWeek)).ToList();
    }
}
