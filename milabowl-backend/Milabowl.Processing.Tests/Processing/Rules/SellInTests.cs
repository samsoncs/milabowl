using FluentAssertions;
using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class SellInTests : MilaRuleTest<SellIn>
{
    [Fact]
    public void Should_award_1_point_if_subs_in_outscore_subs_out()
    {
        var state = new MilaGameWeekStateBuilder()
            .WithSubsIn(TestStateFactory.GetSub().RuleFor(r => r.TotalPoints, 20))
            .WithSubsOut(TestStateFactory.GetSub().RuleFor(r => r.TotalPoints, 10))
            .Build();

        var result = Rule.Calculate(state);

        result.Points.Should().Be(1);
    }

    [Fact]
    public void Should_award_0_points_if_subs_in_do_not_outscore_subs_out()
    {
        var state = new MilaGameWeekStateBuilder()
            .WithSubsIn(TestStateFactory.GetSub().RuleFor(r => r.TotalPoints, 10))
            .WithSubsOut(TestStateFactory.GetSub().RuleFor(r => r.TotalPoints, 20))
            .Build();

        var result = Rule.Calculate(state);

        result.Points.Should().Be(0);
    }
}
