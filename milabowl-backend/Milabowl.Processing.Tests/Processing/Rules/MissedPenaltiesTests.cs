using FluentAssertions;
using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class MissedPenaltiesTests : MilaRuleTest<MissedPenalties>
{
    [Fact]
    public void Should_return_0_if_no_penalties_missed()
    {
        var state = StateFactory.GetMilaGameWeekState([
            StateFactory.GetPlayer().RuleFor(x => x.PenaltiesMissed, 0).Generate()
        ]).Generate();
        var result = Rule.Calculate(state);
        result.Points.Should().Be(0);
    }

    [Fact]
    public void Should_return_1_69_for_one_missed_penalty()
    {
        var state = StateFactory.GetMilaGameWeekState([
            StateFactory.GetPlayer().RuleFor(x => x.PenaltiesMissed, 1).Generate()
        ]).Generate();
        var result = Rule.Calculate(state);
        result.Points.Should().Be(1.69m);
    }

    [Fact]
    public void Should_double_points_for_captain()
    {
        var state = StateFactory.GetMilaGameWeekState([
            StateFactory.GetCaptain().RuleFor(x => x.PenaltiesMissed, 1).Generate()
        ]).Generate();
        var result = Rule.Calculate(state);
        result.Points.Should().Be(3.38m);
    }

    [Fact]
    public void Should_sum_points_for_all_missed_penalties_in_lineup()
    {
        var state = StateFactory.GetMilaGameWeekState([
            StateFactory.GetPlayer().RuleFor(x => x.PenaltiesMissed, 1).Generate(),
            StateFactory.GetPlayer().RuleFor(x => x.PenaltiesMissed, 2).Generate(),
            StateFactory.GetPlayer().RuleFor(x => x.PenaltiesMissed, 0).Generate(),
            StateFactory.GetPlayer().RuleFor(x => x.PenaltiesMissed, 1).Generate()
        ]).Generate();
        var result = Rule.Calculate(state);

        result.Points.Should().Be(5.07m);
    }
}
