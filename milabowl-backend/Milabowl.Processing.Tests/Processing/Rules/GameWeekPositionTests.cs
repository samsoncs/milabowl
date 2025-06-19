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
        var userScore = 10;
        var oppScore = 20;
        var opponent = StateFactory.GetMilaGameWeekState(userTotalScore: oppScore).Generate().User;
        var state = StateFactory.GetMilaGameWeekState(userTotalScore: userScore, opponents: new List<UserState> { opponent }).Generate();

        var result = Rule.Calculate(state);

        result.Points.Should().Be(0);
    }

    [Fact]
    public void Should_award_half_point_for_second_place()
    {
        var userScore = 20;
        var oppScore = 10;
        var opponent = StateFactory.GetMilaGameWeekState(userTotalScore: oppScore).Generate().User;
        var state = StateFactory.GetMilaGameWeekState(userTotalScore: userScore, opponents: new List<UserState> { opponent }).Generate();

        var result = Rule.Calculate(state);

        result.Points.Should().Be(0.5m);
    }
}
