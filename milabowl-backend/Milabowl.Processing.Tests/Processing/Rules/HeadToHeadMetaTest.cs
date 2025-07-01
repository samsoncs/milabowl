using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;
using Shouldly;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class HeadToHeadMetaTest : MilaRuleTest<HeadToHeadMeta>
{
    [Fact]
    public void Should_get_2_points_if_head_to_head_opponent_beaten_with_less_than_2_points()
    {
        var headToHeadEvent = TestStateFactory
            .GetHeadToHeadEvent()
            .RuleFor(r => r.DidWin, true)
            .RuleFor(r => r.Points, 10);
        var headToHead = TestStateFactory
            .GetHeadToHead(10, 8)
            .RuleFor(r => r.CurrentUser, headToHeadEvent)
            .Generate();
        var state = new ManagerGameWeekStateBuilder().WithHeadToHead(headToHead).Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(2);
    }

    [Fact]
    public void Should_get_0_points_if_head_to_head_opponent_beaten_with_more_than_2_points()
    {
        var headToHeadEvent = TestStateFactory
            .GetHeadToHeadEvent()
            .RuleFor(r => r.DidWin, true)
            .RuleFor(r => r.Points, 11);
        var headToHead = TestStateFactory
            .GetHeadToHead(11, 8)
            .RuleFor(r => r.CurrentUser, headToHeadEvent)
            .Generate();
        var state = new ManagerGameWeekStateBuilder().WithHeadToHead(headToHead).Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(0);
    }

    [Fact]
    public void Should_get_no_points_if_head_to_head_opponent_is_draw()
    {
        var headToHeadEvent = TestStateFactory
            .GetHeadToHeadEvent()
            .RuleFor(r => r.DidWin, false)
            .RuleFor(r => r.Points, 10);
        var headToHead = TestStateFactory
            .GetHeadToHead(10, 10)
            .RuleFor(r => r.CurrentUser, headToHeadEvent)
            .Generate();
        var state = new ManagerGameWeekStateBuilder().WithHeadToHead(headToHead).Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(0);
    }
}
