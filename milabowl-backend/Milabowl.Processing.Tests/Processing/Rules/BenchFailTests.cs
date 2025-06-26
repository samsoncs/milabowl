using FluentAssertions;
using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class BenchFailTests : MilaRuleTest<BenchFail>
{
    [Fact]
    public void Should_return_minus_1_for_5_points_on_bench()
    {
        var state = new MilaGameWeekStateBuilder()
            .WithLineup(TestStateFactory.GetBenchPlayer().RuleFor(r => r.TotalPoints, 5))
            .Build();

        var result = Rule.Calculate(state);

        result.Points.Should().Be(-1);
    }

    [Fact]
    public void Should_return_minus_2_for_10_points_on_bench()
    {
        var state = new MilaGameWeekStateBuilder()
            .WithLineup(TestStateFactory.GetBenchPlayer().RuleFor(r => r.TotalPoints, 10))
            .Build();

        var result = Rule.Calculate(state);

        result.Points.Should().Be(-2);
    }

    [Fact]
    public void Should_return_0_for_less_than_5_points_on_bench()
    {
        var state = new MilaGameWeekStateBuilder()
            .WithLineup(TestStateFactory.GetBenchPlayer().RuleFor(r => r.TotalPoints, 4))
            .Build();

        var result = Rule.Calculate(state);

        result.Points.Should().Be(0);
    }

    [Fact]
    public void Should_return_0_when_no_bench_players()
    {
        var state = new MilaGameWeekStateBuilder().WithLineup().Build();
        var result = Rule.Calculate(state);
        result.Points.Should().Be(0);
    }
}
