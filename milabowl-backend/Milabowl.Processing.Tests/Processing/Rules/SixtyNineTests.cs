using FluentAssertions;
using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class SixtyNineTests: MilaRuleTest<SixtyNine>
{
    [Fact]
    public void Should_get_6_point_9_points_if_team_scores_69()
    {
        var state = StateFactory.GetMilaGameWeekState(
            [
                StateFactory
                    .GetCaptain()
                    .RuleFor(r => r.TotalPoints, 3),
                StateFactory
                    .GetPlayer()
                    .RuleFor(r => r.TotalPoints, 63)
            ]
        );

        var result = Rule.Calculate(state);

        result.Points.Should().Be(6.9m);
    }

    [Fact]
    public void Should_not_get_points_if_team_scores_other_than_69()
    {
        var state = StateFactory.GetMilaGameWeekState(
            [
                StateFactory
                    .GetPlayer()
                    .RuleFor(r => r.TotalPoints, 3),
                StateFactory
                    .GetPlayer()
                    .RuleFor(r => r.TotalPoints, 63),
            ]
        );

        var result = Rule.Calculate(state);

        result.Points.Should().Be(0);
    }
}
