using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;
using Shouldly;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class MinusIsPlusTests : MilaRuleTest<MinusIsPlus>
{
    [Fact]
    public void Should_get_plus_points_for_all_minus_points()
    {
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(
                TestStateFactory.GetPlayer().RuleFor(r => r.TotalPoints, f => f.Random.Int(-10, -1))
            )
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(-1 * state.Lineup[0].TotalPoints);
    }

    [Fact]
    public void Should_get_doubled_plus_points_for_captain_minus_points()
    {
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(
                TestStateFactory
                    .GetCaptain()
                    .RuleFor(r => r.TotalPoints, f => f.Random.Int(-10, -1))
            )
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(-1 * 2 * state.Lineup[0].TotalPoints);
    }

    [Fact]
    public void Should_sum_all_minus_points()
    {
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(
                TestStateFactory.GetCaptain().RuleFor(r => r.TotalPoints, -5),
                TestStateFactory.GetPlayer().RuleFor(r => r.TotalPoints, -32),
                TestStateFactory.GetPlayer().RuleFor(r => r.TotalPoints, 0),
                TestStateFactory.GetPlayer().RuleFor(r => r.TotalPoints, 4),
                TestStateFactory.GetBenchPlayer().RuleFor(r => r.TotalPoints, -10)
            )
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(42);
    }
}
