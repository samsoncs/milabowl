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
        var opponent = new MilaGameWeekStateBuilder()
            .WithTotalScore(20)
            .Build();
        var state = new MilaGameWeekStateBuilder()
            .WithTotalScore(10)
            .WithOpponents(opponent)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.Should().Be(0);
    }

    [Fact]
    public void Should_award_half_point_for_second_place()
    {
        var opponent = new MilaGameWeekStateBuilder()
            .WithTotalScore(10)
            .Build();
        var state = new MilaGameWeekStateBuilder()
            .WithOpponents(opponent)
            .WithTotalScore(20)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.Should().Be(0.5m);
    }
}
