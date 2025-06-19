using FluentAssertions;
using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class UniquePlayersTests : MilaRuleTest<UniquePlayers>
{
    [Fact]
    public void Should_award_3_points_if_user_has_highest_weighted_score()
    {
        var userPlayer = StateFactory.GetPlayer().RuleFor(x => x.FantasyPlayerEventId, 1).RuleFor(x => x.TotalPoints, 10).Generate();
        var state = StateFactory.GetMilaGameWeekState([userPlayer]).Generate();

        var result = Rule.Calculate(state);

        result.Points.Should().Be(3);
    }

    [Fact]
    public void Should_award_0_points_if_user_has_lowest_weighted_score()
    {
        var userPlayer = StateFactory.GetPlayer().RuleFor(x => x.FantasyPlayerEventId, 1).RuleFor(x => x.TotalPoints, 5).Generate();
        var state = StateFactory.GetMilaGameWeekState([userPlayer]).Generate();

        var result = Rule.Calculate(state);

        result.Points.Should().Be(0);
    }
}

