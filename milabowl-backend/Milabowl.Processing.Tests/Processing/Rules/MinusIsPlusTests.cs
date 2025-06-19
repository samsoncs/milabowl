using FluentAssertions;
using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class MinusIsPlusTests: MilaRuleTest<MinusIsPlus>
{
    [Fact]
    public void Should_get_plus_points_for_all_minus_points()
    {
        var state = StateFactory.GetMilaGameWeekState(
            [
                StateFactory.GetPlayer().RuleFor(r => r.TotalPoints, f => f.Random.Int(-10, -1)),
            ]
        ).Generate();

        var result = Rule.Calculate(state);

        result.Points.Should().Be(-1*state.User.Lineup[0].TotalPoints);
    }

    [Fact]
    public void Should_get_doubled_plus_points_for_captain_minus_points()
    {
        var state = StateFactory.GetMilaGameWeekState(
            [
                StateFactory.GetCaptain().RuleFor(r => r.TotalPoints, f => f.Random.Int(-10, -1)),
            ]
        ).Generate();

        var result = Rule.Calculate(state);

        result.Points.Should().Be(-1*2*state.User.Lineup[0].TotalPoints);
    }

    [Fact]
    public void Should_sum_all_minus_points()
    {
        var state = StateFactory.GetMilaGameWeekState(
            [
                StateFactory.GetCaptain().RuleFor(r => r.TotalPoints, -5),
                StateFactory.GetPlayer().RuleFor(r => r.TotalPoints, -32),
                StateFactory.GetPlayer().RuleFor(r => r.TotalPoints, 0),
                StateFactory.GetPlayer().RuleFor(r => r.TotalPoints, 4),
                StateFactory.GetBenchPlayer().RuleFor(r => r.TotalPoints, -10),
            ]
        );

        var result = Rule.Calculate(state);

        result.Points.Should().Be(42);
    }
}
