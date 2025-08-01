using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;
using Shouldly;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class RedCardsTests : MilaRuleTest<RedCards>
{
    [Fact]
    public void Should_get_six_points_for_playing_player_with_red_card()
    {
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(TestStateFactory.GetPlayer().RuleFor(r => r.RedCards, 1))
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(6);
    }

    [Fact]
    public void Should_get_twelve_points_for_playing_captain_with_red_card()
    {
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(TestStateFactory.GetCaptain().RuleFor(r => r.RedCards, 1))
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(12);
    }

    [Fact]
    public void Should_get_no_points_for_playing_player_without_red_card()
    {
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(TestStateFactory.GetPlayer().RuleFor(r => r.RedCards, 0))
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(0);
    }

    [Fact]
    public void Should_get_no_points_for_benched_player_with_red_card()
    {
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(TestStateFactory.GetBenchPlayer().RuleFor(r => r.RedCards, 1))
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(0);
    }

    [Fact]
    public void Should_sum_players_card()
    {
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(
                TestStateFactory.GetPlayer().RuleFor(r => r.RedCards, 1),
                TestStateFactory.GetPlayer().RuleFor(r => r.RedCards, 1)
            )
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(12);
    }
}
