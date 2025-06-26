using FluentAssertions;
using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class SixtyNineSubTests : MilaRuleTest<SixtyNineSub>
{
    [Fact]
    public void Should_award_2_69_if_any_player_played_69_minutes()
    {
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(TestStateFactory.GetPlayer().RuleFor(x => x.Minutes, 69))
            .Build();

        var result = Rule.Calculate(state);

        result.Points.Should().Be(2.69m);
    }

    [Fact]
    public void Should_double_points_if_captain_played_69_minutes()
    {
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(TestStateFactory.GetCaptain().RuleFor(x => x.Minutes, 69))
            .Build();

        var result = Rule.Calculate(state);

        result.Points.Should().Be(5.38m);
    }

    [Fact]
    public void Should_award_0_if_no_player_played_69_minutes()
    {
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(TestStateFactory.GetPlayer().RuleFor(x => x.Minutes, 68))
            .Build();

        var result = Rule.Calculate(state);

        result.Points.Should().Be(0);
    }
}

