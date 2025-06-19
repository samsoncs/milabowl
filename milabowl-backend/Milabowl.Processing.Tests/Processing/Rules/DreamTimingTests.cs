using FluentAssertions;
using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class DreamTimingTests: MilaRuleTest<DreamTiming>
{
    [Fact]
    public void Should_get_3_points_for_dream_timing()
    {
        var state = StateFactory.GetMilaGameWeekState(
            [
                StateFactory.GetPlayer().RuleFor(r => r.TotalPoints, 3),
                StateFactory.GetCaptain().RuleFor(r => r.TotalPoints, 6),
            ]
        ).Generate();

        var result = Rule.Calculate(state);

        result.Points.Should().Be(3);
    }
}
