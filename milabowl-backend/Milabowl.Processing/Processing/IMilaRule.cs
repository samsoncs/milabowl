using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing;

public record MilaRuleResult(string RuleName, string RuleShortName, decimal Points);

public interface IMilaRule
{
    MilaRuleResult Calculate(UserGameWeek userGameWeek);
}

public abstract class MilaRule : IMilaRule
{
    protected abstract string ShortName { get; }
    protected abstract decimal CalculatePoints(UserGameWeek userGameWeek);

    public MilaRuleResult Calculate(UserGameWeek userGameWeek)
    {
        return new MilaRuleResult(GetType().Name, ShortName, CalculatePoints(userGameWeek));
    }
}
