using FluentAssertions;
using Milabowl.Processing.DataImport.Models;
using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class GameWeekPositionTests : MilaRuleTest<GameWeekPosition>
{
    [Fact]
    public void Should_award_0_points_for_last_place()
    {
        var user = new UserStateBuilder()
            .WithTotalScore(10)
            .Build();
        var opponent = new UserStateBuilder()
            .WithTotalScore(20)
            .Build();
        var state = new MilaGameWeekStateBuilder()
            .WithUser(user)
            .WithOpponents(opponent)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.Should().Be(0);
    }

    [Fact]
    public void Should_award_half_point_for_second_place()
    {
        var user = new UserStateBuilder()
            .WithTotalScore(20)
            .Build();
        var opponent = new UserStateBuilder()
            .WithTotalScore(10)
            .Build();
        var state = new MilaGameWeekStateBuilder()
            .WithUser(user)
            .WithOpponents(opponent)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.Should().Be(0.5m);
    }
}
