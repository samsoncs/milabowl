using Milabowl.Processing.DataImport;

namespace Milabowl.Processing.Processing;

public record MilaRuleResult(string RuleName, string RuleShortName, decimal Points);

public interface IMilaRule
{
    MilaRuleResult Calculate(UserGameWeek userGameWeek);
}
