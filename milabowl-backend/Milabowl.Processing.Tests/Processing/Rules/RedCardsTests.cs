using FluentAssertions;
using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class RedCardsTests: MilaRuleTest<RedCards>
{
    [Fact]
    public void Should_get_six_points_for_playing_player_with_red_card()
    {
        var state = StateFactory.GetMilaGameWeekState([
            StateFactory.GetPlayer().RuleFor(r => r.RedCards, 1),
        ]);

        var result = Rule.Calculate(state);

        result.Points.Should().Be(6);
    }

    [Fact]
    public void Should_get_twelve_points_for_playing_captain_with_red_card()
    {
        var state = StateFactory.GetMilaGameWeekState([
            StateFactory.GetCaptain().RuleFor(r => r.RedCards, 1),
        ]);

        var result = Rule.Calculate(state);

        result.Points.Should().Be(12);
    }

    [Fact]
    public void Should_get_no_points_for_playing_player_without_red_card()
    {
        var state = StateFactory.GetMilaGameWeekState([
            StateFactory.GetPlayer().RuleFor(r => r.RedCards, 0),
        ]);

        var result = Rule.Calculate(state);

        result.Points.Should().Be(0);
    }

    [Fact]
    public void Should_get_no_points_for_benched_player_with_red_card()
    {
        var state = StateFactory.GetMilaGameWeekState([
            StateFactory.GetBenchPlayer().RuleFor(r => r.RedCards, 1),
        ]);

        var result = Rule.Calculate(state);

        result.Points.Should().Be(0);
    }

    [Fact]
    public void Should_sum_players_card()
    {
        var state = StateFactory.GetMilaGameWeekState([
            StateFactory.GetPlayer().RuleFor(r => r.RedCards, 1),
            StateFactory.GetPlayer().RuleFor(r => r.RedCards, 1),
        ]);

        var result = Rule.Calculate(state);

        result.Points.Should().Be(12);
    }

}
