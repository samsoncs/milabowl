using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing;

public record MilaRuleResult(
    string RuleName,
    string RuleShortName,
    string Description,
    decimal Points,
    string? Reasoning
);

public interface IMilaRule
{
    MilaRuleResult Calculate(ManagerGameWeekState userGameWeek);
}

public record RulePoints(decimal Points, string? Reasoning);

public abstract class MilaRule : IMilaRule
{
    protected abstract string ShortName { get; }
    protected abstract string Description { get; }
    protected abstract RulePoints CalculatePoints(ManagerGameWeekState userGameWeek);

    public MilaRuleResult Calculate(ManagerGameWeekState userGameWeek)
    {
        var rulePoints = CalculatePoints(userGameWeek);
        return new MilaRuleResult(
            GetType().Name,
            ShortName,
            Description,
            rulePoints.Points,
            rulePoints.Reasoning
        );
    }
}
