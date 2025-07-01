using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;
using Shouldly;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class CapFailScoreTests : MilaRuleTest<CapFailScore>
{
    [Fact]
    public void Should_return_minus_1_if_captain_scores_less_than_5()
    {
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(
                TestStateFactory
                    .GetCaptain()
                    .RuleFor(x => x.TotalPoints, f => f.Random.Int(-10, 4))
                    .Generate()
            )
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(-1);
    }

    [Fact]
    public void Should_return_0_if_captain_scores_5_or_more()
    {
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(
                TestStateFactory
                    .GetCaptain()
                    .RuleFor(x => x.TotalPoints, f => f.Random.Int(5, 20))
                    .Generate()
            )
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(0);
    }
}
