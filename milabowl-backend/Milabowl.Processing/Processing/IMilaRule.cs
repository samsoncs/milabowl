using Milabowl.Processing.DataImport;
using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing;

public record MilaRuleResult(string RuleName, string RuleShortName, decimal Points);

public interface IMilaRule
{
    MilaRuleResult Calculate(UserGameWeek userGameWeek);
}
