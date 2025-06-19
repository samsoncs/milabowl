using FluentAssertions;
using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class HeadBrosTests : MilaRuleTest<HeadBros>
{
    [Fact]
    public void Should_award_points_if_user_and_opponent_have_highest_combined_score()
    {
        var userHeadToHead = StateFactory
            .GetHeadToHead()
            .RuleFor(x => x.CurrentUser.Points, 30)
            .RuleFor(x => x.Opponent.Points, 40)
            .Generate();

        var opp1HeadToHead = StateFactory
            .GetHeadToHead()
            .RuleFor(x => x.CurrentUser.Points, 20)
            .RuleFor(x => x.Opponent.Points, 10)
            .Generate();

        var opp2HeadToHead = StateFactory
            .GetHeadToHead()
            .RuleFor(x => x.CurrentUser.Points, 30)
            .RuleFor(x => x.Opponent.Points, 10)
            .Generate();

        var userState = StateFactory.GetMilaGameWeekState(
            userHeadToHead: userHeadToHead,
            opponents: [
                StateFactory.GetMilaGameWeekState(userHeadToHead: opp1HeadToHead).Generate().User,
                StateFactory.GetMilaGameWeekState(userHeadToHead: opp2HeadToHead).Generate().User
            ]
            ).Generate();

        var result = Rule.Calculate(userState);

        result.Points.Should().Be(2.69m);
    }

    [Fact]
    public void Should_award_0_if_user_and_opponent_do_not_have_highest_combined_score()
    {
        var userHeadToHead = StateFactory
            .GetHeadToHead()
            .RuleFor(x => x.CurrentUser.Points, 10)
            .RuleFor(x => x.Opponent.Points, 10)
            .Generate();
        var oppHeadToHead = StateFactory
            .GetHeadToHead()
            .RuleFor(x => x.CurrentUser.Points, 30)
            .RuleFor(x => x.Opponent.Points, 40)
            .Generate();
        var userState = StateFactory
            .GetMilaGameWeekState(
                userHeadToHead: userHeadToHead,
                opponents: [StateFactory.GetMilaGameWeekState(userHeadToHead: oppHeadToHead).Generate().User]
            )
            .Generate();

        var result = Rule.Calculate(userState);

        result.Points.Should().Be(0);
    }
}
