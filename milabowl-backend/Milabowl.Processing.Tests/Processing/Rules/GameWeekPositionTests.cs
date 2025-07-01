using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;
using Shouldly;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class GameWeekPositionTests : MilaRuleTest<GameWeekPosition>
{
    [Fact]
    public void Should_award_0_points_for_last_place()
    {
        var opponent = new ManagerGameWeekStateBuilder().WithTotalScore(20).Build();
        var state = new ManagerGameWeekStateBuilder()
            .WithTotalScore(10)
            .WithOpponents(opponent)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(0);
    }

    [Fact]
    public void Should_award_half_point_for_second_place()
    {
        var opponent = new ManagerGameWeekStateBuilder().WithTotalScore(10).Build();
        var state = new ManagerGameWeekStateBuilder()
            .WithOpponents(opponent)
            .WithTotalScore(20)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(0.5m);
    }
}
