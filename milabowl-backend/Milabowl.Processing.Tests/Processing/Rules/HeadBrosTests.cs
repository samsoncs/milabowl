using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;
using Shouldly;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class HeadBrosTests : MilaRuleTest<HeadBros>
{
    [Fact]
    public void Should_award_points_if_user_and_opponent_have_highest_combined_score()
    {
        var opponent1 = new ManagerGameWeekStateBuilder()
            .WithHeadToHead(
                TestStateFactory.GetHeadToHead(20, 10)
            )
            .Build();

        var opponent2 = new ManagerGameWeekStateBuilder()
            .WithHeadToHead(
                TestStateFactory.GetHeadToHead(30, 10)
            )
            .Build();

        var state = new ManagerGameWeekStateBuilder()
            .WithHeadToHead(TestStateFactory.GetHeadToHead(30, 40))
            .WithOpponents(opponent1, opponent2)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(2.69m);
    }

    [Fact]
    public void Should_award_0_if_user_and_opponent_do_not_have_highest_combined_score()
    {
        var opponent = new ManagerGameWeekStateBuilder()
            .WithHeadToHead(
                TestStateFactory.GetHeadToHead(30, 10)
            )
            .Build();

        var state = new ManagerGameWeekStateBuilder()
            .WithHeadToHead(TestStateFactory.GetHeadToHead(10, 20))
            .WithOpponents(opponent)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(0);
    }
}
