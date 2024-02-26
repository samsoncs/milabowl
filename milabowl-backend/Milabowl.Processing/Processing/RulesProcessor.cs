using Milabowl.Processing.DataImport;

namespace Milabowl.Processing.Processing;

public interface IRulesProcessor
{
    void CalculateForUserGameWeek(UserGameWeek userGameWeek);
}

public class RulesProcessor: IRulesProcessor
{
    private readonly IEnumerable<IMilaRule> _rules;
    
    public RulesProcessor(IEnumerable<IMilaRule> rules)
    {
        _rules = rules;
    }

    public void CalculateForUserGameWeek(UserGameWeek userGameWeek)
    {
        var results = _rules.Select(r => r.Calculate(userGameWeek)).ToList();
    }
    

}